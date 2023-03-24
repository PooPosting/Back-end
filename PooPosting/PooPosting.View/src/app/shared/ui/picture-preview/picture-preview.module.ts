import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PicturePreviewComponent} from "./picture-preview.component";
import {RouterModule} from "@angular/router";
import {DateAgoModule} from "../../utils/pipes/date-ago/date-ago.module";
import {CardModule} from "primeng/card";
import {ButtonModule} from "primeng/button";
import {DialogModule} from "primeng/dialog";



@NgModule({
  declarations: [
    PicturePreviewComponent
  ],
  exports: [
    PicturePreviewComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    DateAgoModule,
    CardModule,
    ButtonModule,
    DialogModule
  ]
})
export class PicturePreviewModule { }
