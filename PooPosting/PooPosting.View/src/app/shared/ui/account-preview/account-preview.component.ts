import {Component, Input, OnInit} from '@angular/core';
import {AccountDto} from "../../utils/dtos/AccountDto";

@Component({
  selector: 'app-account-preview',
  templateUrl: './account-preview.component.html',
  styleUrls: ['./account-preview.component.scss']
})
export class AccountPreviewComponent {
  @Input() account!: AccountDto;
}
