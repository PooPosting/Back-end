import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PictureSkeletonComponent} from "./picture-skeleton.component";
import {CardModule} from "primeng/card";
import {SkeletonModule} from "primeng/skeleton";



@NgModule({
  declarations: [
    PictureSkeletonComponent
  ],
  imports: [
    CommonModule,
    CardModule,
    SkeletonModule
  ],
  exports: [
    PictureSkeletonComponent
  ]
})
export class PictureSkeletonModule { }
