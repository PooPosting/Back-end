import {Component,OnInit} from '@angular/core';
import {MessageService} from "primeng/api";
import {Clipboard} from "@angular/cdk/clipboard";
import {PictureDetailsServiceService} from "../../../shared/state/picture-details-service.service";
import {AppCacheService} from "../../../shared/state/app-cache.service";
import {environment} from "../../../../environments/environment";
import {PictureDto} from "../../../shared/utils/dtos/PictureDto";
import {CommentDto} from "../../../shared/utils/dtos/CommentDto";
import {map, Observable, Subscription} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {LocationServiceService} from "../../../shared/helpers/location-service.service";
import {PictureLikesService} from "../../../shared/data-access/picture/picture-likes.service";
import {PictureService} from "../../../shared/data-access/picture/picture.service";

@Component({
  selector: 'app-picture-details-modal',
  templateUrl: './picture-details-modal.component.html',
  styleUrls: ['./picture-details-modal.component.scss']
})
export class PictureDetailsModalComponent implements OnInit {

  id: Observable<string>;
  picture: PictureDto | null = null;
  comments: CommentDto[] = [];
  visible: boolean = true;
  isLoggedOn: boolean = false;
  recentlyRefreshed: boolean = false;

  shareUrl: string = `${environment.appWebUrl}/picture/`;
  showSettings: boolean = false;


  constructor(
    private pictureService: PictureService,
    private likeService: PictureLikesService,
    private messageService: MessageService,
    private pictureDetailsService: PictureDetailsServiceService,
    private clipboard: Clipboard,
    private cacheService: AppCacheService,
    private route: ActivatedRoute,
    private locationService: LocationServiceService,
    private router: Router
  ) {
    this.id = route.params.pipe(map(p => p['id']));
  }

  ngOnInit(): void {
    this.isLoggedOn = this.cacheService.cachedUserInfo != undefined;
    this.refreshPicture();
  }

  copyUrl(textToCopy: string) {
    this.messageService.clear();
    this.clipboard.copy(textToCopy);
    this.messageService.add({
      severity: 'success',
      summary: 'Sukces!',
      detail: 'Pomyślnie skopiowano adres obrazka!',
    })
  }
  like() {
    this.likeService.likePicture(this.picture!.id)
      .subscribe(this.likeObserver)
  }
  dislike() {
    this.likeService.dislikePicture(this.picture!.id)
      .subscribe(this.likeObserver)
  }

  likeObserver = {
    next: (v: PictureDto) => {
      this.picture = v;
    },
  }

  refreshPicture() {
    this.recentlyRefreshed = true;
    this.fetchPicture();
    setTimeout(() => {
      this.recentlyRefreshed = false;
    }, 10000)
  }

  fetchPicture() {
    let sub: Subscription = this.id.subscribe({
      next: (id) => {
        this.shareUrl = `${environment.appWebUrl}/picture/${id}`;
        let sub: Subscription = this.pictureService.getPictureById(id)
          .subscribe({
            next: (pic) => {
              this.picture = pic;
            },
            error: () => this.locationService.goError404(),
            complete: () => sub.unsubscribe()
          });
      },
      error: () => this.locationService.goError404(),
      complete: () => sub.unsubscribe()
    });
  }

  pictureChanged(dto: PictureDto) {
    this.picture = dto;
    this.showSettings = false;
  }

  pictureDeleted() {
    this.locationService.goBack();
  }

  close() {
    this.visible = false;
    setTimeout(() => {
      this.router.navigate(["../../../"]);
    }, 90)
  }

}
