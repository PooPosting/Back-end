import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {PostPictureComponent} from "./post-picture.component";
import {StepsModule} from "primeng/steps";
import {RouterModule} from "@angular/router";
import { PostCropComponent } from './post-crop/post-crop.component';
import { PostDetailsComponent } from './post-details/post-details.component';
import { PostOverviewComponent } from './post-overview/post-overview.component';
import {PictureCropperModule} from "../../../shared/ui/picture-cropper/picture-cropper.module";
import {ButtonModule} from "primeng/button";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {InputTextModule} from "primeng/inputtext";
import {ChipsModule} from "primeng/chips";
import {InputTextareaModule} from "primeng/inputtextarea";
import {CardModule} from "primeng/card";
import {TagModule} from "primeng/tag";
import {PostPictureRoutingModule} from "./post-picture-routing.module";

@NgModule({
  declarations: [
    PostPictureComponent,
    PostCropComponent,
    PostDetailsComponent,
    PostOverviewComponent
  ],
  exports: [
    PostPictureComponent
  ],
  imports: [
    PostPictureRoutingModule,
    CommonModule,
    StepsModule,
    RouterModule,
    PictureCropperModule,
    ButtonModule,
    ReactiveFormsModule,
    FormsModule,
    InputTextModule,
    ChipsModule,
    InputTextareaModule,
    CardModule,
    TagModule
  ]
})
export class PostPictureModule { }
