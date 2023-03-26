import {Component, Input, OnInit} from '@angular/core';
import {HttpServiceService} from "../../../shared/data-access/http-service.service";
import {LikeResult} from "../../../shared/utils/dtos/LikeResult";
import {PictureDto} from "../../../shared/utils/dtos/PictureDto";

@Component({
  selector: 'app-picture-slider-card',
  templateUrl: './picture-slider-card.component.html',
  styleUrls: ['./picture-slider-card.component.scss']
})
export class PictureSliderCardComponent implements OnInit {
  @Input() picture!: PictureDto;
  @Input() index!: number;
  @Input() showCommentCount: boolean = false;
  @Input() showLikeCount: boolean = false;

  constructor(
    private httpService: HttpServiceService,
  ) { }

  ngOnInit(): void {
  }

  like(){
    // this.httpService.patchPictureLikeRequest(this.picture.id)
    //   .subscribe(this.likeObserver)
  }
  dislike(){
    // this.httpService.patchPictureDislikeRequest(this.picture.id)
    //   .subscribe(this.likeObserver)
  }

  likeObserver = {
    next: (v: LikeResult) => {
      this.picture.likeCount = v.likeCount;
      this.picture.dislikeCount = v.dislikeCount;
      this.picture.likeState = v.likeState;
    },
  }

}
