import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {AccountBannerComponent} from "./account-banner.component";


@NgModule({
  declarations: [
    AccountBannerComponent
  ],
  exports: [
    AccountBannerComponent
  ],
  imports: [
    CommonModule,
  ]
})
export class AccountBannerModule { }
