import {Component, Input, OnInit} from '@angular/core';
import {PictureModel} from "../../../../../Models/ApiModels/Get/PictureModel";
import {HttpServiceService} from "../../../../../Services/http/http-service.service";
import {LikeResult} from "../../../../../Models/ApiModels/Patch/LikeResult";

@Component({
  selector: 'app-picture-slider-card',
  templateUrl: './picture-slider-card.component.html',
  styleUrls: ['./picture-slider-card.component.scss']
})
export class PictureSliderCardComponent implements OnInit {
  @Input() picture!: PictureModel;
  @Input() index!: number;
  @Input() showCommentCount: boolean = false;
  @Input() showLikeCount: boolean = false;

  constructor(
    private httpService: HttpServiceService,
  ) { }

  ngOnInit(): void {
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
    next: (v: LikeResult) => {
      this.picture.likeCount = v.likeCount;
      this.picture.dislikeCount = v.dislikeCount;
      this.picture.likes = v.likes;
      this.picture.likeState = v.likeState;
    },
  }

}
