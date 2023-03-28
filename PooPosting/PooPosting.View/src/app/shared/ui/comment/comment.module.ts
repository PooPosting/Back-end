import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {CommentComponent} from "./comment.component";
import {RippleModule} from "primeng/ripple";
import {ButtonModule} from "primeng/button";
import {RouterModule} from "@angular/router";
import {DateAgoModule} from "../../utils/pipes/date-ago/date-ago.module";



@NgModule({
  declarations: [
    CommentComponent
  ],
  imports: [
    CommonModule,
    RippleModule,
    ButtonModule,
    RouterModule,
    DateAgoModule
  ],
  exports: [
    CommentComponent
  ]
})
export class CommentModule { }
