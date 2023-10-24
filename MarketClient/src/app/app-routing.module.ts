import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutsComponent } from './layouts/layouts.component';
import { HomeComponent } from './home/home.component';
import { FoodComponent } from './food/food.component';
import { FruitVegComponent } from './fruit-veg/fruit-veg.component';
import { BeveragesComponent } from './beverages/beverages.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';

const routes: Routes = [
  {
    path: "",
    component: LayoutsComponent,
    children: [
      {
        path: "",
        component: HomeComponent
      },
      {
        path: "food",
        component: FoodComponent
      },
      {
        path: "fruit-veg",
        component: FruitVegComponent
      },
      {
        path: "beverages",
        component: BeveragesComponent
      },
      {
        path: "shopping-cart",
        component: ShoppingCartComponent
      }
    ]
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
