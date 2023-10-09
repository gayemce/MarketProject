import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  count: number = 0;
  constructor(private translate: TranslateService) {
    // Varsayılan dil ayarı (eğer kullanıcı dilini seçmemişse)
    translate.setDefaultLang('tr');
  }

  changeLanguage(lang: string) {
    this.translate.use(lang);
  }

  onLanguageChange(event: any) {
    this.translate.use(event.target.value);
  }
}


