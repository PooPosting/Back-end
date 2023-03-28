import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentSectionComponent } from './comment-section.component';
import {CommentModule} from "../../ui/comment/comment.module";
import {InputTextareaModule} from "primeng/inputtextarea";
import {ReactiveFormsModule} from "@angular/forms";
import {ButtonModule} from "primeng/button";



@NgModule({
  declarations: [
    CommentSectionComponent
  ],
  exports: [
    CommentSectionComponent
  ],
  imports: [
    CommonModule,
    CommentModule,
    InputTextareaModule,
    ReactiveFormsModule,
    ButtonModule
  ]
})
export class CommentSectionModule { }
