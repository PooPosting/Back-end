import {Component, OnDestroy, OnInit} from '@angular/core';
import {MessageService} from "primeng/api";
import {map, Observable, Subscription} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpServiceService} from "../../../shared/data-access/http-service.service";
import {LocationServiceService} from "../../../shared/helpers/location-service.service";
import {Title} from "@angular/platform-browser";
import {AppCacheService} from "../../../shared/state/app-cache.service";
import {TitleCasePipe} from "@angular/common";
import {PictureDetailsServiceService} from "../../../shared/state/picture-details-service.service";
import {PictureDto} from "../../../shared/utils/dtos/PictureDto";
import {PictureLikesService} from "../../../shared/data-access/picture/picture-likes.service";

@Component({
  selector: 'app-picture-details',
  templateUrl: './picture-details.component.html',
  styleUrls: ['./picture-details.component.scss']
})
export class PictureDetailsComponent implements OnInit, OnDestroy {
  picture!: PictureDto;
  id: Observable<string>;
  isLoggedOn: boolean = false;

  showSettingsFlag: boolean = false;

  private readonly subs = new Subscription();

  constructor(
    private likeService: PictureLikesService,
    private cacheService: AppCacheService,
    private locationService: LocationServiceService,
    private pictureDetailsService: PictureDetailsServiceService,
    private httpService: HttpServiceService,
    private messageService: MessageService,
    private titleCasePipe: TitleCasePipe,
    private route: ActivatedRoute,
    private router: Router,
    private title: Title
  ) {
    this.isLoggedOn = cacheService.getUserLoggedOnState();
    this.id = route.params.pipe(map(p => p['id']));
    this.initialSubscribe();
  }

  ngOnInit() {
    this.subs.add(
      this.pictureDetailsService.pictureDeletedSubject.subscribe({
        next: (val: string) => {
          if (val === this.picture.id) {
            this.cacheService.cachedPictures = this.cacheService.getCachedPictures().filter(p => p.id !== this.picture.id);
            this.locationService.goBack();
          }
        }
      })
    );
    this.subs.add(
      this.pictureDetailsService.pictureChangedSubject.subscribe({
        next: (val: PictureDto) => {
          if (val.id === this.picture.id) {
            this.picture = val;
          }
        }
      })
    );
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  like() {
    this.subs.add(
      this.likeService.likePicture(this.picture.id)
        .subscribe(this.likeObserver)
    );
  }
  dislike(){
    this.subs.add(
      this.likeService.dislikePicture(this.picture.id)
        .subscribe(this.likeObserver)
    );
  }

  return() {
    this.locationService.goBack();
  }

  initialSubscribe() {
    this.subs.add(
      this.id.subscribe({
        next: (val) => {
          this.httpService.getPictureRequest(val).subscribe({
            next: (pic: PictureDto) => {
              this.picture = pic;
              this.title.setTitle(`PooPosting - ${this.titleCasePipe.transform(pic.name)}`);
            },
            error: () => {
              this.router.navigate(['/error404']);
            },
          });
        }
      })
    );
  }

  likeObserver = {
    next: () => {
      this.httpService.getPictureRequest(this.picture.id)
        .subscribe({
          next: (value: PictureDto) => {
            this.picture = value;
          }
      })
    }
  }

  pictureChanged(dto: PictureDto) {
    this.picture = dto;
    this.showSettingsFlag = false;
  }

  pictureDeleted() {
    this.locationService.goBack();
  }

}
