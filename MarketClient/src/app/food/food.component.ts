import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RequestModel } from '../models/request.model';
import { ProductModel } from '../models/product.model';

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

  constructor(private http: HttpClient){
    this.getCategories();
  }

  feedData(){
    this.request.pageSize += 10;
    this.newData = [];
    this.getAll();
  }

  changeCategory(categoryId: number | null = null){
    this.request.categoryId = categoryId;
    this.request.pageSize = 0;
    this.feedData();
  }

  getAll(){
    this.http.post<ProductModel[]>(`https://localhost:7150/api/Products/GetAll/`, this.request)
    .subscribe(res => {
      this.products = res;
    })
  }

  getCategories(){
    this.http.get(`https://localhost:7150/api/Categories/GetAll`)
    .subscribe(res => {
      this.categories = res;
      this.getAll();
    });
  }

}
