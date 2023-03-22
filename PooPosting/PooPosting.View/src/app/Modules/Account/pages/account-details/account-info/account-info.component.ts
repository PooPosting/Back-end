import {Component, Input, OnInit} from '@angular/core';
import {AccountModel} from "../../../../../Models/ApiModels/Get/AccountModel";

@Component({
  selector: 'app-account-info',
  templateUrl: './account-info.component.html',
  styleUrls: ['./account-info.component.scss']
})
export class AccountInfoComponent implements OnInit {

  @Input() account!: AccountModel;

  constructor() { }

  ngOnInit(): void {
  }

}
