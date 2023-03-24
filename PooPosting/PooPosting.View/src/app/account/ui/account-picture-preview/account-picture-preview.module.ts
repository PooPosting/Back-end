import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AccountPicturePreviewComponent} from "./account-picture-preview.component";
@NgModule({
  declarations: [
    AccountPicturePreviewComponent
  ],
  exports: [
    AccountPicturePreviewComponent
  ],
  imports: [
    CommonModule
  ]
})
export class AccountPicturePreviewModule { }
