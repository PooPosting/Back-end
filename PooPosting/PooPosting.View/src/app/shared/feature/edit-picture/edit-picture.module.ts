import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditPictureComponent } from './edit-picture.component';
import {RippleModule} from "primeng/ripple";
import {ButtonModule} from "primeng/button";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ChipsModule} from "primeng/chips";
import {SelectButtonModule} from "primeng/selectbutton";
import {InputTextareaModule} from "primeng/inputtextarea";



@NgModule({
  declarations: [
    EditPictureComponent
  ],
  exports: [
    EditPictureComponent
  ],
  imports: [
    CommonModule,
    RippleModule,
    ButtonModule,
    ReactiveFormsModule,
    ChipsModule,
    FormsModule,
    SelectButtonModule,
    InputTextareaModule
  ]
})
export class EditPictureModule { }
