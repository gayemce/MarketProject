import { Injectable } from '@angular/core';
import { SwalService } from './swal.service';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { PaymentModel } from '../models/payment.model';
import { AuthService } from './auth.service';
import { SetShoppingCartsModel } from '../models/set-shopping-carts.model';
import { ErrorService } from './error.service';
import { DriverService } from './driver.service';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  shoppingCarts: any[] = []; //Ürünler var
  prices: { value: number, currency: string }[] = [];
  count: number = 0;
  total: number = 0;

  /* Daha önceden sepete ürün eklendiyese sayfa refresh edilse de bu ürün sepette görünecek. */
  constructor(
    private swal: SwalService,
    private translate: TranslateService,
    private http: HttpClient,
    private auth: AuthService,
    private error: ErrorService,
  ) {
    this.getAllShoppingCarts();
  }

  changeProductQuantityInShoppingCart(productId: number, quantity: number) {
    if (localStorage.getItem("response")) {
      this.http.get(`https://localhost:7150/api/ShoppingCarts/ChangeProductQuantityInShoppingCart/${productId}/${quantity}`)
        .subscribe({
          next: (res: any) => {
            this.getAllShoppingCarts();
          },
          error: (err: HttpErrorResponse) => {
            this.error.errorHandler(err);
          }
        })
    }
    else {
      if (quantity <= 0) {
        const index = this.shoppingCarts.findIndex(p => p.id == productId);
        this.removeByIndex(index);
      }
      else {
        this.http.get(`https://localhost:7150/api/ShoppingCarts/CheckProductQuantityIsAvailable/${productId}/${quantity}`).subscribe({
          next: (res: any) => {
            this.shoppingCarts.filter(p => p.id === productId)[0].quantity = quantity;
          },
          error: (err: HttpErrorResponse) => {
            this.error.errorHandler(err);
          }
        })
      }
    }
  }


  //Sepetteki ürünleri elde eder
  getAllShoppingCarts() {
    const shoppingCartString = localStorage.getItem("shoppingCarts")
    if (shoppingCartString) {
      const carts: string | null = localStorage.getItem("shoppingCarts");
      if (carts !== null) {
        this.shoppingCarts = JSON.parse(carts);
        this.calcTotal();
      }
    }
    else {
      this.shoppingCarts = [];
    }

    //kullanıcı girişi varsa
    if (localStorage.getItem("response")) {
      this.http.get<SetShoppingCartsModel[]>("https://localhost:7150/api/ShoppingCarts/GetAll/" + this.auth.userId,).subscribe({
        next: (res: any) => {
          this.shoppingCarts = res
          this.calcTotal();

        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
        }
      })
    }

    this.calcTotal();
  }

  calcTotal() {
    this.count = this.shoppingCarts.length;
    this.total = 0;

    const sumMap = new Map<string, number>();

    this.prices = [];
    for (let s of this.shoppingCarts) {
      const newPrice = { value: (s.price.value * s.stock), currency: s.price.currency };
      this.prices.push({ ...newPrice });
    }

    for (const item of this.prices) { //currency değerlerini birbirinden ayırıyor
      const currentSum = sumMap.get(item.currency) || 0;
      sumMap.set(item.currency, currentSum + item.value);
    }

    this.prices = [];
    for (const [currency, sum] of sumMap) {
      this.prices.push({ value: sum, currency: currency });
      console.log(this.prices);
    }

  }

  removeByIndex(index: number) {
    forkJoin({
      doYouWantToDeleted: this.translate.get('remove.doYouWantToDeleted'),
      cancelBtn: this.translate.get('remove.cancelBtn'),
      confirmBtn: this.translate.get('remove.confirmBtn')
    }).subscribe((res) => {
      this.swal.callSwal(res.doYouWantToDeleted, res.cancelBtn, res.confirmBtn, () => {
        if (localStorage.getItem("response")) {
          this.http.get("https://localhost:7150/api/ShoppingCarts/RemoveById/" + this.shoppingCarts[index]?.shoppingCartId).subscribe(res => {
            this.getAllShoppingCarts();
          });
        }
        else {
          this.shoppingCarts.splice(index, 1);
          localStorage.setItem("shoppingCarts", JSON.stringify(this.shoppingCarts)); //güncel halini set et.
          this.count = this.shoppingCarts.length;
          this.calcTotal();
        }
      });
    });
  }

  payment(data: PaymentModel, callBack: (res: any) => void) {
    this.http.post("https://localhost:7150/api/ShoppingCarts/Payment", data)
      .subscribe({
        next: (res: any) => {
          callBack(res);
        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
        }
      })
  }
}
