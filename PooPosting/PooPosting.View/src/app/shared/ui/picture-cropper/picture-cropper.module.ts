import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PictureCropperComponent} from "./picture-cropper.component";
import {AngularCropperjsModule} from "angular-cropperjs";
import {FileUploadModule} from "primeng/fileupload";



@NgModule({
  declarations: [
    PictureCropperComponent
  ],
  exports: [
    PictureCropperComponent
  ],
  imports: [
    CommonModule,
    AngularCropperjsModule,
    FileUploadModule
  ]
})
export class PictureCropperModule { }
