import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PrimeNgModule} from "../Prime-ng/prime-ng.module";
import {SharedModule} from "../Shared/shared.module";

import {MyAccountComponent} from "./pages/my-account/my-account.component";
import {AccountDetailsComponent} from "./pages/account-details/account-details.component";
import {AccountRoutingModule} from "./account-routing.module";
import { AccountBannerComponent } from './pages/account-details/account-banner/account-banner.component';
import { AccountPicturePreviewComponent } from './pages/account-details/account-picture-preview/account-picture-preview.component';
import { AccountSettingsComponent } from './pages/account-details/account-settings/account-settings.component';
import { AccountAdminSettingsComponent } from './pages/account-details/account-admin-settings/account-admin-settings.component';
import { AccountInfoComponent } from './pages/account-details/account-info/account-info.component';
import {ReactiveFormsModule} from "@angular/forms";
import {AngularCropperjsModule} from "angular-cropperjs";

@NgModule({
  declarations: [
    MyAccountComponent,
    AccountDetailsComponent,
    AccountBannerComponent,
    AccountPicturePreviewComponent,
    AccountSettingsComponent,
    AccountAdminSettingsComponent,
    AccountInfoComponent,
  ],
    imports: [
        CommonModule,
        PrimeNgModule,
        SharedModule,
        ReactiveFormsModule,
        AccountRoutingModule,
        AngularCropperjsModule,
    ]
})
export class AccountModule { }
