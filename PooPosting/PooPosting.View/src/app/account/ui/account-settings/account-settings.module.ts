import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AccountSettingsComponent} from "./account-settings.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {PictureCropperModule} from "../../../shared/ui/picture-cropper/picture-cropper.module";
import {InputTextareaModule} from "primeng/inputtextarea";
import {ButtonModule} from "primeng/button";
import {KeyFilterModule} from "primeng/keyfilter";
import {OverlayPanelModule} from "primeng/overlaypanel";
import {ListboxModule} from "primeng/listbox";
import {ChipsModule} from "primeng/chips";



@NgModule({
  declarations: [
    AccountSettingsComponent
  ],
  exports: [
    AccountSettingsComponent
  ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PictureCropperModule,
        InputTextareaModule,
        ButtonModule,
        KeyFilterModule,
        OverlayPanelModule,
        ListboxModule,
        ChipsModule
    ]
})
export class AccountSettingsModule { }
