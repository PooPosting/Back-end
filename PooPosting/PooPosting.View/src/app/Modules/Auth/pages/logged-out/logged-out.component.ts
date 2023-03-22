import { Component } from '@angular/core';
import {LocationServiceService} from "../../../../Services/helpers/location-service.service";
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-logged-out',
  templateUrl: './logged-out.component.html',
  styleUrls: ['./logged-out.component.scss']
})
export class LoggedOutComponent {

  constructor(
    private locationService: LocationServiceService,
    private title: Title
  ) {
    this.title.setTitle(`PicturesUI - Wylogowano cię`);
  }

  back(): void {
    this.locationService.goBack();
  }

}
