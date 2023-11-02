import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from 'src/app/service/swal.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor(
    private http: HttpClient,
    private swal: SwalService,
    public translate: TranslateService,
    private router: Router
  ) {

  }

  signUp(form: NgForm) {
    if (form.valid) {
      this.http.post("https://localhost:7150/api/Auth/Register", {
        name: form.controls["name"].value,
        lastname: form.controls["lastname"].value,
        username: form.controls["username"].value,
        email: form.controls["email"].value,
        password: form.controls["password"].value
      })
        .subscribe(res => {
          this.translate.get("res.message").subscribe(res => {
            this.swal.callToast(res, "success");
          })
          this.router.navigateByUrl("/login");
        })
    }
  }
}
