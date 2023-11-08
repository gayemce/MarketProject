import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/service/auth.service';
import { DriverService } from 'src/app/service/driver.service';
import { ShoppingCartService } from 'src/app/service/shopping-cart.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  language: string = "en";

  constructor(
    private translate: TranslateService,
    public shopping: ShoppingCartService,
    public auth: AuthService,
    private router: Router,
    public driver: DriverService
  ) {
    if (localStorage.getItem("language")) {
      this.language = localStorage.getItem("language") as string;
    }
    
    translate.setDefaultLang(this.language);
    this.driver.showDriverPopup();
  }

  ngAfterViewInit() {
     // Bu metot bileşenin görünümü tamamen oluşturulduktan sonra çalışır
    this.changeLanguage();
  }

  switchLanguage(event: any) {
    localStorage.setItem("language", event.target.value);
    this.language = event.target.value
    this.translate.use(this.language);
    location.reload();
  }

  changeLanguage() {
    let language: HTMLElement | any = document.getElementById('language');
    let toggleLanguage: HTMLElement | any = document.getElementById('toggleLanguage');
    let storedLanguage = localStorage.getItem("language")

    if(storedLanguage === "tr"){
    language.innerText = "Türkçe";
    toggleLanguage.innerText = "Türkçe";
    }
    else{
      language.innerText = "English";
      toggleLanguage.innerText = "English";
    }
  }


  logout() {
    localStorage.removeItem("response");
    this.shopping.getAllShoppingCarts();
    this.router.navigateByUrl("/login");
  }
}


