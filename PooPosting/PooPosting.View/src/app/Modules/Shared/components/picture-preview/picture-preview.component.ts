import {Component, Input, OnInit} from '@angular/core';
import { PictureModel } from 'src/app/Models/ApiModels/Get/PictureModel';
import { HttpServiceService } from 'src/app/Services/http/http-service.service';
import {PictureDetailsServiceService} from "../../../../Services/data/picture-details-service.service";

@Component({
  selector: 'app-picture-preview',
  templateUrl: './picture-preview.component.html',
  styleUrls: ['./picture-preview.component.scss']
})
export class PicturePreviewComponent implements OnInit {
  @Input() picture!: PictureModel;
  @Input() isLoggedOn!: boolean;
  isDeleted: boolean = false;

  showShare: boolean = false;

  constructor(
    private httpService: HttpServiceService,
    private pictureDetailsService: PictureDetailsServiceService,
  ) { }

  ngOnInit(): void {
    this.pictureDetailsService.pictureDeletedSubject.subscribe({
      next: (val: string) => {
        if (val === this.picture.id) {
          this.isDeleted = true;
        }
      }
    })
    this.pictureDetailsService.pictureChangedSubject.subscribe({
      next: (val: PictureModel) => {
        if (val.id === this.picture.id) {
          this.picture = val;
        }
      }
    });
  }


  like(){
    this.httpService.patchPictureLikeRequest(this.picture.id)
      .subscribe(this.likeObserver)
  }
  dislike(){
    this.httpService.patchPictureDislikeRequest(this.picture.id)
      .subscribe(this.likeObserver)
  }

  likeObserver = {
    next: (v: PictureModel) => {
      this.picture = v;
    },
  }

  showDetails() {
    this.pictureDetailsService.modalTriggerSubject.next(this.picture.id);
  }



}
