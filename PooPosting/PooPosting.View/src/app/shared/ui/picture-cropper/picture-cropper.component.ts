import {Component, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {CropperSettings} from "ngx-image-cropper/lib/interfaces/cropper.settings";
import {ImageCroppedEvent, ImageCropperComponent} from "ngx-image-cropper";

@Component({
  selector: 'app-picture-cropper',
  templateUrl: './picture-cropper.component.html',
  styleUrls: ['./picture-cropper.component.scss']
})
export class PictureCropperComponent {
  @ViewChild("cropper") cropperComponent!: ImageCropperComponent;
  @Output() onCroppedFileSubmit: EventEmitter<Blob> = new EventEmitter<Blob>();
  @Output() onReadyChange: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() onCropperDataChanged: EventEmitter<string> = new EventEmitter<string>();

  awaitSubmit = false;

  @Input() config: Partial<CropperSettings> = {
    aspectRatio: 1,
    alignImage: "center",
    resizeToWidth: 256
  }
  @Input() showDefaultSubmitButton: boolean = true;
  @Input() buttonLabel: string = "Zatwierdź";
  @Input() cropperDataUrl?: string | null;

  imageFile?: File;

  imageCropped(event: ImageCroppedEvent) {
    if (event.base64) {
      this.cropperDataUrl = event.base64;
      this.onReadyChange.emit(true);
      this.onCropperDataChanged.emit(event.base64!);
    }
  }
  onFileSelect(event: any) {
    this.imageFile = event.currentFiles[0];
  }
  onFileCancel() {
    this.cropperDataUrl = "";
    this.imageFile = undefined;
    this.onReadyChange.emit(false);
    this.onCropperDataChanged.emit("");
  }
  public crop() {
    if (this.cropperDataUrl) {
      this.awaitSubmit = true;
      const blob = this.dataURItoBlob(this.cropperDataUrl!);
      this.onCroppedFileSubmit.emit(blob);
      this.onReadyChange.emit(false);
    }
  }

  private dataURItoBlob(dataURI: string) {
    const byteString = atob(dataURI.split(',')[1]);
    const mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], {type: mimeString});
  }

}
