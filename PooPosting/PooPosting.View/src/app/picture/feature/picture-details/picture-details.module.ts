import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PictureDetailsRoutingModule } from './picture-details-routing.module';
import {PictureDetailsComponent} from "./picture-details.component";
import {CardModule} from "primeng/card";
import {TagModule} from "primeng/tag";
import {ButtonModule} from "primeng/button";
import {DialogModule} from "primeng/dialog";


@NgModule({
  declarations: [
    PictureDetailsComponent
  ],
  exports: [
    PictureDetailsComponent
  ],
  imports: [
    CommonModule,
    PictureDetailsRoutingModule,
    CardModule,
    TagModule,
    ButtonModule,
    DialogModule
  ]
})
export class PictureDetailsModule { }
