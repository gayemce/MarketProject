import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SetShoppingCartsModel } from 'src/app/models/set-shopping-carts.model';
import { AuthService } from 'src/app/service/auth.service';
import { ShoppingCartService } from 'src/app/service/shopping-cart.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(
    private http: HttpClient,
    private router: Router,
    private auth: AuthService,
    private shopping: ShoppingCartService
  ) {

  }

  signIn(form: NgForm) {
    if (form.valid) {
      this.http.post("https://localhost:7150/api/Auth/Login",
        {
          usernameOrEmail: form.controls["usernameOrEmail"].value,
          password: form.controls["password"].value
        })
        .subscribe((res: any) => {
          localStorage.setItem("response", JSON.stringify(res));
          this.auth.isAuthentication();

          const request: SetShoppingCartsModel[] = [];

          if(this.shopping.shoppingCarts.length > 0){
            for(let s of this.shopping.shoppingCarts){
              const cart = new SetShoppingCartsModel();
              cart.productId = s.id;
              cart.userId = this.auth.userId;
              cart.price = s.price;
              cart.quantity = 1;

              request.push(cart);
            }

          this.http.post("https://localhost:7150/api/ShoppingCarts/setShoppingCartsFromLocalStorage", request).subscribe(res => {
            localStorage.removeItem("shoppingCarts")
            this.shopping.checkLocalStorageForShoppingCarts();
          });
        }

          this.router.navigateByUrl("/")
        })
    }
  }
}
