import {Component, Input} from '@angular/core';
import {PicturePreviewModel} from "../../../../../Models/ApiModels/Get/PicturePreviewModel";
import {PictureDetailsServiceService} from "../../../../../Services/data/picture-details-service.service";

@Component({
  selector: 'app-account-picture-preview',
  templateUrl: './account-picture-preview.component.html',
  styleUrls: ['./account-picture-preview.component.scss']
})
export class AccountPicturePreviewComponent {

  @Input() picturePreview!: PicturePreviewModel;
  @Input() name!: string;

  constructor(
    private pictureDetailsService: PictureDetailsServiceService
  ) {
  }

  showPictureModal() {
    this.pictureDetailsService.modalTriggerSubject.next(this.picturePreview.id);
  }

}
