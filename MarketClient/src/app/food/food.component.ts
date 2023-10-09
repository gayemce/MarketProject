import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RequestModel } from '../models/request.model';

@Component({
  selector: 'app-food',
  templateUrl: './food.component.html',
  styleUrls: ['./food.component.css']
})
export class FoodComponent {
  response: any;
  categories: any = [];
  pageNumbers: number[] = [];
  request: RequestModel = new RequestModel();

  constructor(private http: HttpClient){
    this.getAll();
    this.getCategories();
  }

  getAll(pageNumber = 1){
    this.request.pageNumber = pageNumber;
    this.http.post(`https://localhost:7150/api/Products/GetAll/`, this.request)
    .subscribe(res => {
      this.response = res;
      this.setPageNumber();
    })
  }

  getCategories(){
    this.http.get(`https://localhost:7289/api/Categories/GetAll`)
    .subscribe(res => this.categories)
  }

  setPageNumber(){
    this.pageNumbers = [];
    for(let i = 0; i < this.response.totalPageCount; i++){
      this.pageNumbers.push(i+1)
    }
  }
}
