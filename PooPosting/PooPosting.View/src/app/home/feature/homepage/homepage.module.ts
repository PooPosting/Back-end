import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomepageRoutingModule } from './homepage-routing.module';
import {HomepageComponent} from "./homepage.component";
import {PictureSkeletonModule} from "../../../shared/ui/picture-skeleton/picture-skeleton.module";
import {PicturePreviewModule} from "../../../shared/ui/picture-preview/picture-preview.module";


@NgModule({
  declarations: [
    HomepageComponent
  ],
  exports: [
    HomepageComponent
  ],
  imports: [
    CommonModule,
    HomepageRoutingModule,
    PictureSkeletonModule,
    PicturePreviewModule
  ]
})
export class HomepageModule { }
