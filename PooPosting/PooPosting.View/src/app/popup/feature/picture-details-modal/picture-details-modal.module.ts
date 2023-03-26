import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PictureDetailsModalRoutingModule } from './picture-details-modal-routing.module';
import {PictureDetailsModalComponent} from "./picture-details-modal.component";
import {TagModule} from "primeng/tag";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {InputTextareaModule} from "primeng/inputtextarea";
import {ButtonModule} from "primeng/button";
import {CommentModule} from "../../ui/comment/comment.module";
import {ChipsModule} from "primeng/chips";
import {SelectButtonModule} from "primeng/selectbutton";
import {RippleModule} from "primeng/ripple";
import {DialogModule} from "primeng/dialog";


@NgModule({
  declarations: [
    PictureDetailsModalComponent
  ],
  exports: [
    PictureDetailsModalComponent
  ],
  imports: [
    CommonModule,
    PictureDetailsModalRoutingModule,
    TagModule,
    ReactiveFormsModule,
    InputTextareaModule,
    ButtonModule,
    CommentModule,
    ChipsModule,
    SelectButtonModule,
    FormsModule,
    RippleModule,
    DialogModule
  ]
})
export class PictureDetailsModalModule { }
