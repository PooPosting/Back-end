import {Component, ViewChild} from '@angular/core';
import {LocationServiceService} from "../../../../../Services/helpers/location-service.service";
import {PostPictureServiceService} from "../../../../../Services/helpers/post-picture-service.service";
import {PictureCropperComponent} from "../../../../Shared/components/picture-cropper/picture-cropper.component";

@Component({
  selector: 'app-post-crop',
  templateUrl: './post-crop.component.html',
  styleUrls: ['./post-crop.component.scss']
})
export class PostCropComponent {

  next: boolean = false;

  @ViewChild("pictureCropperComponent") cropperComponent!: PictureCropperComponent;

  constructor(
    private locationService: LocationServiceService,
    private postPictureService: PostPictureServiceService
  ) {
  }

  return() {
    this.locationService.goBack();
  }

  saveFile(file: Blob) {
    this.postPictureService.file = file;
  }

  saveCropperData(data: string) {
    this.postPictureService.cropperDataUrl = data;
  }

  getCropperData() {
    const data = this.postPictureService.cropperDataUrl;
    (data) ? this.next = true : this.next = false;
    return data;
  }

  cropFile() {
    this.cropperComponent.crop();
  }

}
