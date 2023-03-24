import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PostCropRoutingModule } from './post-crop-routing.module';
import {PostCropComponent} from "./post-crop.component";
import {PictureCropperModule} from "../../../../shared/ui/picture-cropper/picture-cropper.module";
import {ButtonModule} from "primeng/button";
@NgModule({
  declarations: [
    PostCropComponent
  ],
  exports: [
    PostCropComponent
  ],
  imports: [
    CommonModule,
    PostCropRoutingModule,
    PictureCropperModule,
    ButtonModule
  ]
})
export class PostCropModule { }
