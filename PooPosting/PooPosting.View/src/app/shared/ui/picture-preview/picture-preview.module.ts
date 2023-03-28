import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PicturePreviewComponent} from "./picture-preview.component";
import {RouterModule} from "@angular/router";
import {DateAgoModule} from "../../utils/pipes/date-ago/date-ago.module";
import {CardModule} from "primeng/card";
import {ButtonModule} from "primeng/button";
import {DialogModule} from "primeng/dialog";
import {ShareModalModule} from "../../../popup/feature/share-modal/share-modal.module";
import {VoteButtonModule} from "../../feature/vote-button/vote-button.module";



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
        DialogModule,
        ShareModalModule,
        VoteButtonModule
    ]
})
export class PicturePreviewModule { }
