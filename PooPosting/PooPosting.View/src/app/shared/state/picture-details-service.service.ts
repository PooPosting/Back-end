import { Injectable } from '@angular/core';
import {Subject} from "rxjs";
import {HttpServiceService} from "../data-access/http-service.service";
import {PictureDto} from "../utils/dtos/PictureDto";

@Injectable({
  providedIn: 'root'
})
export class PictureDetailsServiceService {

  modalTriggerSubject: Subject<string> = new Subject<string>();
  showModalSubject: Subject<PictureDto> = new Subject<PictureDto>();
  pictureChangedSubject: Subject<PictureDto> = new Subject<PictureDto>();
  pictureDeletedSubject: Subject<string> = new Subject<string>();

  constructor(
    private httpService: HttpServiceService
  ) {
    this.modalTriggerSubject.subscribe({
      next: (val) => {
        this.httpService.getPictureRequest(val)
          .subscribe({
            next: (pic) => {
              this.showModalSubject.next(pic);
            }
          })
      }
    })
  }

  pictureChanged(picture: PictureDto) {
    this.pictureChangedSubject.next(picture);
  }

  pictureDeleted(pictureId: string) {
    this.pictureDeletedSubject.next(pictureId);
  }

}
