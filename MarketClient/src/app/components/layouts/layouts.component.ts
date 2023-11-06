import { Component } from '@angular/core';
import { DriverService } from 'src/app/service/driver.service';

@Component({
  selector: 'app-layouts',
  templateUrl: './layouts.component.html',
  styleUrls: ['./layouts.component.css']
})
export class LayoutsComponent {

  constructor(
    public driver: DriverService
  ){}
}
