import { Injectable } from '@angular/core';
import {Subject} from "rxjs";
import {HttpServiceService} from "../http/http-service.service";
import {PictureModel} from "../../Models/ApiModels/Get/PictureModel";

@Injectable({
  providedIn: 'root'
})
export class PictureDetailsServiceService {

  modalTriggerSubject: Subject<string> = new Subject<string>();
  showModalSubject: Subject<PictureModel> = new Subject<PictureModel>();
  pictureChangedSubject: Subject<PictureModel> = new Subject<PictureModel>();
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

  pictureChanged(picture: PictureModel) {
    this.pictureChangedSubject.next(picture);
  }

  pictureDeleted(pictureId: string) {
    this.pictureDeletedSubject.next(pictureId);
  }

}
