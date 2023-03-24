import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AccountPicturePreviewModule} from "../../ui/account-picture-preview/account-picture-preview.module";
import {AccountAdminSettingsModule} from "../../ui/account-admin-settings/account-admin-settings.module";
import {AccountDetailsComponent} from "./account-details.component";
import {AccountInfoModule} from "../../ui/account-info/account-info.module";
import {AccountDetailsRoutingModule} from "./account-details-routing.module";
import {AccountBannerModule} from "../../ui/account-banner/account-banner.module";
import {AccountSettingsModule} from "../../ui/account-settings/account-settings.module";
import {ButtonModule} from "primeng/button";
import {RippleModule} from "primeng/ripple";
import {DialogModule} from "primeng/dialog";


@NgModule({
  declarations: [
    AccountDetailsComponent,
  ],
  imports: [
    CommonModule,
    AccountDetailsRoutingModule,
    AccountBannerModule,
    AccountPicturePreviewModule,
    AccountInfoModule,
    AccountAdminSettingsModule,
    AccountSettingsModule,
    ButtonModule,
    RippleModule,
    DialogModule
  ]
})
export class AccountDetailsModule { }
