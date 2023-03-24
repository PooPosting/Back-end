import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {PostPictureComponent} from "./post-picture.component";
import {StepsModule} from "primeng/steps";
import {PostCropModule} from "./post-crop/post-crop.module";
import {RouterModule} from "@angular/router";


@NgModule({
  declarations: [
    PostPictureComponent
  ],
  exports: [
    PostPictureComponent
  ],
  imports: [
    CommonModule,
    PostCropModule,
    StepsModule,
    RouterModule
  ]
})
export class PostPictureModule { }
