import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutsComponent } from './layouts/layouts.component';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './layouts/navbar/navbar.component';
import { FoodComponent } from './food/food.component';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { FruitVegComponent } from './fruit-veg/fruit-veg.component';
import { BeveragesComponent } from './beverages/beverages.component';
import { FooterComponent } from './layouts/footer/footer.component';
import { FormsModule } from '@angular/forms';
import { CategoryPipe } from './pipes/category.pipe';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { IconControlDirective } from './directives/icon-control.directive';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';
import { TrCurrencyPipe } from 'tr-currency';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    LayoutsComponent,
    HomeComponent,
    NavbarComponent,
    FoodComponent,
    FruitVegComponent,
    BeveragesComponent,
    FooterComponent,
    CategoryPipe,
    IconControlDirective,
    ShoppingCartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    TrCurrencyPipe,
    SweetAlert2Module,
    InfiniteScrollModule,
    FormsModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
