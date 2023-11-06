import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from './swal.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor(
    private translate: TranslateService,
    private swal: SwalService
  ) { }

  errorHandler(err: HttpErrorResponse) {
    console.log(err);

    switch (err.status) {
      case 0:
        this.translate.get("apiNotAvailable").subscribe(res => {
          this.swal.callToast(res, "error");
        });
        break;

      case 400:
        this.swal.callToast(err.error.message, "error");
        break;

      case 404:
        this.translate.get("ApiNotFound").subscribe(res => {
          this.swal.callToast(res, "error");
        });
        break;

      case 500:
        this.swal.callToast(err.error.message, "error");
        break;

      default:
        this.translate.get("errorStatusNotFound").subscribe(res => {
          this.swal.callToast(res, "error");
        });
        break;
    }
  }
}
