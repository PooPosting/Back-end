import {Component, Input, OnInit} from '@angular/core';
import {AccountDto} from "../../../../../Models/Dtos/AccountDto";

@Component({
  selector: 'app-account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.scss']
})
export class AccountInfoComponent implements OnInit {

  @Input() account!: AccountDto;

  constructor() { }

  ngOnInit(): void {
  }

}
