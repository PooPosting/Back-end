import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PictureDetailsRoutingModule } from './picture-details-routing.module';
import {PictureDetailsComponent} from "./picture-details.component";
import {CardModule} from "primeng/card";
import {TagModule} from "primeng/tag";
import {ButtonModule} from "primeng/button";
import {DialogModule} from "primeng/dialog";
import {VoteButtonModule} from "../../../shared/feature/vote-button/vote-button.module";
import {CommentSectionModule} from "../../../shared/feature/comment-section/comment-section.module";
import {EditPictureModule} from "../../../shared/feature/edit-picture/edit-picture.module";


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
    DialogModule,
    VoteButtonModule,
    CommentSectionModule,
    EditPictureModule
  ]
})
export class PictureDetailsModule { }
