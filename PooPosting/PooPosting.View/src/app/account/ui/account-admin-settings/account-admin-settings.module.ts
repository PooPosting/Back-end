import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {AccountAdminSettingsComponent} from "./account-admin-settings.component";
import {FormsModule} from "@angular/forms";
import {ChipsModule} from "primeng/chips";
import {RippleModule} from "primeng/ripple";
import {ButtonModule} from "primeng/button";


@NgModule({
  declarations: [
    AccountAdminSettingsComponent
  ],
  exports: [
    AccountAdminSettingsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ChipsModule,
    RippleModule,
    ButtonModule
  ]
})
export class AccountAdminSettingsModule { }
