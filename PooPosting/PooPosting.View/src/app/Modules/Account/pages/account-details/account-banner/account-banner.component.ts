import {Component, Input } from '@angular/core';
import {AccountModel} from "../../../../../Models/ApiModels/Get/AccountModel";

@Component({
  selector: 'app-account-banner',
  templateUrl: './account-banner.component.html',
  styleUrls: ['./account-banner.component.scss']
})
export class AccountBannerComponent {

  @Input() account!: AccountModel;

}
