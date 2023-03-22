import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AccountModel} from "../../../../../Models/ApiModels/Get/AccountModel";

@Component({
  selector: 'app-account-admin-settings',
  templateUrl: './account-admin-settings.component.html',
  styleUrls: ['./account-admin-settings.component.scss']
})
export class AccountAdminSettingsComponent {

  @Input() account!: AccountModel
  @Output() onBan: EventEmitter<void> = new EventEmitter<void>();

  banPhrase: string = "";

  banAccount() {
    this.banPhrase = "";
    this.onBan.emit();
  }

}
