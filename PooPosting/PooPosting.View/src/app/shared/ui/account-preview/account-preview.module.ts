import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AccountPreviewComponent} from "./account-preview.component";
import {CardModule} from "primeng/card";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [
    AccountPreviewComponent
  ],
  exports: [
    AccountPreviewComponent
  ],
  imports: [
    CommonModule,
    CardModule,
    RouterModule,
  ]
})
export class AccountPreviewModule { }
