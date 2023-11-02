import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  token: string = "";
  userId: number = 0;
  userName: string = "";

  constructor() { }

  isAuthentication(){
    const responseString = localStorage.getItem("response");
    if(responseString){
      const responseJson = JSON.parse(responseString);
      this.token = responseJson.token; //token değişkenini başka yerde kullanabilmek için
      this.userId = responseJson.userId;
      this.userName = responseJson.userName;
      return true;
    }

    return false;
  }
}
