import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PictureCropperComponent} from "./picture-cropper.component";
import {FileUploadModule} from "primeng/fileupload";
import {ImageCropperModule} from "ngx-image-cropper";



@NgModule({
  declarations: [
    PictureCropperComponent
  ],
  exports: [
    PictureCropperComponent
  ],
    imports: [
        CommonModule,
        FileUploadModule,
        ImageCropperModule
    ]
})
export class PictureCropperModule { }
