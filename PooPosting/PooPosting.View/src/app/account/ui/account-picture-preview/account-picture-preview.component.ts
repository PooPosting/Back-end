import {Component, Input} from '@angular/core';
import {PicturePreviewDto} from "../../../shared/utils/dtos/PicturePreviewDto";
import {PictureDetailsServiceService} from "../../../shared/state/picture-details-service.service";

@Component({
  selector: 'app-account-picture-preview',
  templateUrl: './account-picture-preview.component.html',
  styleUrls: ['./account-picture-preview.component.scss']
})
export class AccountPicturePreviewComponent {

  @Input() picturePreview!: PicturePreviewDto;
  @Input() name!: string;

  constructor(
    private pictureDetailsService: PictureDetailsServiceService
  ) {
  }

  showPictureModal() {
    this.pictureDetailsService.modalTriggerSubject.next(this.picturePreview.id);
  }

}
