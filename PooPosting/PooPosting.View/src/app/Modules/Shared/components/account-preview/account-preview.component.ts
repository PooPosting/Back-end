import {Component, Input, OnInit} from '@angular/core';
import {AccountModel} from "../../../../Models/ApiModels/Get/AccountModel";

@Component({
  selector: 'app-account-preview',
  templateUrl: './account-preview.component.html',
  styleUrls: ['./account-preview.component.scss']
})
export class AccountPreviewComponent {
  @Input() account!: AccountModel;
}
