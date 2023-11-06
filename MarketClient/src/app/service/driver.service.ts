import { Injectable } from '@angular/core';
import { driver } from "driver.js";

@Injectable({
  providedIn: 'root'
})
export class DriverService {
  isPopupShow: boolean = false;
  processBar: number = 0;
  interval: any; 

  constructor() {
    setTimeout(() => {
      this.changePopupShow();
      this.interval = setInterval(() => {
        console.log(this.processBar);
        this.processBar += 2;
      }, 100)
    }, 1000);

    setTimeout(() => {
      clearInterval(this.interval) //interval durduruluna kadar devam eder
      if (this.isPopupShow) {
        this.changePopupShow();
      }
    }, 7000);

  }

  changePopupShow() {
    this.isPopupShow = !this.isPopupShow;
  }

  showDriver() {
    const driverObj = driver({
      popoverClass: "driverjs-theme",
      showProgress: true,
      steps: [
        {
          element: "#driverNavbar",
          popover: {
            title: 'Navbar',
            description: 'Buradan istediğiniz menüyü seçebilirsiniz'
          }
        },
        {
          element: '#languageSelector',
          popover: {
            title: 'Language',
            description: 'Dil değişimi yapabilirsiniz'
          }
        },
        {
          element: '#driverShoppingCart',
          popover: {
            title: 'Shopping Cart',
            description: 'Dilediğiniz ürünü sepetinize ekleyebilirsiniz'
          }
        },
        {
          element: "#driverDropdownMenu",
          popover: {
            title: 'Dropdown Menu',
            description: 'Profilinize bakmak ve çıkış yapmak için buraya tıklayabilirsiniz'
          }
        }
      ]
    });

    driverObj.drive();

    this.changePopupShow();

  }
}
