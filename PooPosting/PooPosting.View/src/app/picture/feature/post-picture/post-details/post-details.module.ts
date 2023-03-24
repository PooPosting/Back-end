import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PostDetailsRoutingModule } from './post-details-routing.module';
import {PostDetailsComponent} from "./post-details.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ChipsModule} from "primeng/chips";
import {InputTextareaModule} from "primeng/inputtextarea";
import {ButtonModule} from "primeng/button";


@NgModule({
  declarations: [
    PostDetailsComponent
  ],
  exports: [
    PostDetailsComponent
  ],
  imports: [
    CommonModule,
    PostDetailsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    ChipsModule,
    InputTextareaModule,
    ButtonModule
  ]
})
export class PostDetailsModule { }
