import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { RequestModel } from '../../models/request.model';
import { ProductModel } from '../../models/product.model';
import { ShoppingCartService } from '../../service/shopping-cart.service';
import Swal from 'sweetalert2'
import { SwalService } from '../../service/swal.service';
import { TranslateService } from '@ngx-translate/core';
import { AddShoppingCartModel } from 'src/app/models/add-shopping-cart.model';
import { AuthService } from 'src/app/service/auth.service';
import { ErrorService } from 'src/app/service/error.service';

@Component({
  selector: 'app-food',
  templateUrl: './food.component.html',
  styleUrls: ['./food.component.css']
})
export class FoodComponent {
  products: ProductModel[] = [];
  categories: any = [];
  pageNumbers: number[] = [];
  request: RequestModel = new RequestModel();
  searchCategory: string = "";
  newData: any[] = [];
  loaderDatas = [1, 2, 3, 4, 5, 6, 7, 8, 9];
  isLoading: boolean = true;


  constructor(
    private http: HttpClient,
    private shopping: ShoppingCartService,
    private swal: SwalService,
    private auth: AuthService,
    private translate: TranslateService,
    private error: ErrorService
  ) {
    /* Kullanıcı girişi yapıldığın kaldığı kategoriden ve yüklenen verilerden devam eder */
    if (localStorage.getItem("request")) {
      const requestString: any = localStorage.getItem("request");
      const requestObj = JSON.parse(requestString)
      this.request = requestObj;
    }
    this.getCategories();
  }

  addShoppingCart(product: ProductModel) {
    //Kullanıcı girişi yapıldıysa
    if (localStorage.getItem("response")) {

      const data: AddShoppingCartModel = new AddShoppingCartModel();
      data.productId = product.id;
      data.price = product.price;
      data.quantity = 1;
      data.userId = this.auth.userId;

      this.http.post("https://localhost:7150/api/ShoppingCarts/Add", data).subscribe({
        next: (res: any) => {
          this.shopping.getAllShoppingCarts();
          this.translate.get("addProductInShoppingCartIsSuccessful").subscribe(res => {
            this.swal.callToast(res);
            // product.stock -= 1;
          });
        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
        }
      });
    }

    //kullanıcı girişi yapılmadıysa
    else {
      if (product.stock < 1) { //stock kontrolü
        this.translate.get("productQuantityIsNotEnough").subscribe(res => {
          this.swal.callToast(res, "error");
        })
      }
      else {
        const checkProductIsAlreadyExists = this.shopping.shoppingCarts.filter(p => p.id == product.id)[0];

        //ürün sepette zaten varsa bir artırır
        if (checkProductIsAlreadyExists !== undefined) {
          this.shopping.shoppingCarts.filter(p => p.id == product.id)[0].quantity += 1;
        }

        //yoksa bir adet ekler
        else {
          const newProduct = { ...product };
          newProduct.stock = 1;
          this.shopping.shoppingCarts.push(newProduct);
        }

        this.shopping.calcTotal();
        localStorage.setItem("shoppingCarts", JSON.stringify(this.shopping.shoppingCarts));
        this.translate.get("addProductInShoppingCartIsSuccessful").subscribe(res => {
          this.swal.callToast(res);
        });
        // product.stock -=1;
      }

    }
  }

  feedData() {
    this.request.pageSize += 10;
    this.newData = [];
    this.getAll();
  }

  changeCategory(categoryId: number | null = null) {
    this.request.categoryId = categoryId;
    this.request.pageSize = 0;
    this.feedData();
  }

  getAll() {
    // this.isLoading = true;
    this.http.post<ProductModel[]>(`https://localhost:7150/api/Products/GetAll/`, this.request)
      .subscribe({
        next: (res: any) => {
          this.products = res;
          this.isLoading = false;
          localStorage.setItem("request", JSON.stringify(this.request));
        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
        }
      });
  }

  getCategories() {
    // this.isLoading = true;
    this.http.get(`https://localhost:7150/api/Categories/GetAll`)
      .subscribe({
        next: (res: any) => {
          this.categories = res;
          this.getAll();
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.error.errorHandler(err);
        }
      });
  }
}