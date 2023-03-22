import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AccountDto} from "../../../../../Models/Dtos/AccountDto";

@Component({
  selector: 'app-account-admin-settings',
  templateUrl: './account-admin-settings.component.html',
  styleUrls: ['./account-admin-settings.component.scss']
})
export class AccountAdminSettingsComponent {

  @Input() account!: AccountDto
  @Output() onBan: EventEmitter<void> = new EventEmitter<void>();

  banPhrase: string = "";

  banAccount() {
    this.banPhrase = "";
    this.onBan.emit();
  }

}
