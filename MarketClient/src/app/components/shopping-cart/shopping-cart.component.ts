import { Component } from '@angular/core';
import { ShoppingCartService } from '../../service/shopping-cart.service';
import { TranslateService } from '@ngx-translate/core';
import { PaymentModel } from '../../models/payment.model';
import { Cities, Countries } from 'src/app/constants/address';
import { SwalService } from 'src/app/service/swal.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent {
  [key: string]: any;

  language: string = "en";
  selectedTab: number = 1;
  request: PaymentModel = new PaymentModel();
  countries = Countries;
  cities = Cities;
  isSameAddress: boolean = true;
  cartNumber1: string = "5890";
  cartNumber2: string = "0400";
  cartNumber3: string = "0000";
  cartNumber4: string = "0016"; //
  expireMonthAndYear: string = "2025-07";
  selectedCurrencyForPayment: string = "₺";

  constructor(
    public shopping: ShoppingCartService,
    private translate: TranslateService,
    private swal: SwalService
  ) {
    if (localStorage.getItem("language")) {
      this.language = localStorage.getItem("language") as string;
    }

    this.request.products = this.shopping.shoppingCarts; //ürünler listesini ShoppingCarta eşitler
    // this.shopping.calcTotal();
  }

  changeTab(tabNumber: number) {
    this.selectedTab = tabNumber;
  }

  setSelectedPaymentCurrency(currency: string) {
    this.selectedCurrencyForPayment = currency;
    const newProducts = this.shopping.shoppingCarts.filter(p => p.price.currency === this.selectedCurrencyForPayment);
    this.request.products = newProducts;
  }

  payment() { 
    this.request.paymentCard.expireMonth = this.expireMonthAndYear.substring(5);
    this.request.paymentCard.expireYear = this.expireMonthAndYear.substring(0,4);
    this.request.paymentCard.cardNumber = this.cartNumber1 + this.cartNumber2 + this.cartNumber3 + this.cartNumber4
    this.request.buyer.registrationAddress = this.request.shippingAddress.description;
    this.request.buyer.city = this.request.shippingAddress.city;
    this.request.buyer.country = this.request.shippingAddress.country;
    this.shopping.payment(this.request, (res)=> {
      const btn = document.getElementById("paymentModalCloseBtn");
      btn?.click();
      const remainShoopingCarts = this.shopping.shoppingCarts.filter(p=> p.price.currency !== this.selectedCurrencyForPayment);
      localStorage.setItem("shoppingCarts", JSON.stringify(remainShoopingCarts));      
      this.shopping.checkLocalStorageForShoppingCarts();
      this.translate.get("paymentIsSuccessful").subscribe(translate => {
        this.swal.callToast(translate,"success");
      });      
    })
  }

  changeIsSameAddress(){
    if(this.isSameAddress){
      this.request.billingAddress = {...this.request.shippingAddress}
    }
  }

  gotoNextInputIf4Lenght(inputCount: string = "", value: string = ""){
    this[`cartNumber${inputCount}`] = value.replace(/[^0-9]/g,"");
    value = value.replace(/[^0-9]/g,"");

    if(value.length === 4){
      if(inputCount === "4"){
        const el = document.getElementById("expireMonthAndYear");
        el?.focus();
      } else{
        const id: string = `cartNumber${+inputCount + 1}`
        const el: HTMLInputElement = document.getElementById(id) as HTMLInputElement
        el.focus();
      }
    }
  }

  setExpireMonthAndYear(){
    //sadece sayıları kabul eder
    this.expireMonthAndYear = this.expireMonthAndYear.replace(/[^0-9]/g, "");

    //eğer stringin uzunluğu 2'den büyükse, 2. ve 3. karakter arasına "/" ekler
    if(this.expireMonthAndYear.length > 2){
      this.expireMonthAndYear = this.expireMonthAndYear.substring(0,2) + "/" + this.expireMonthAndYear.substring(2);
    }

    //ayın 01 ile 12 arası da olup olmadığını kontrol eder
    if(this.expireMonthAndYear.length >= 2){
      const month = parseInt(this.expireMonthAndYear.substring(0,2));
      if(month === 0){
        this.expireMonthAndYear = "01" + this.expireMonthAndYear.substring(2);
      } else if(month > 12){
        this.expireMonthAndYear = "12" + this.expireMonthAndYear.substring(2);
      }
    }

    if(this.expireMonthAndYear.length > 4){
      const el = document.getElementById("cvc");
      el?.focus();
    }
  }
}
