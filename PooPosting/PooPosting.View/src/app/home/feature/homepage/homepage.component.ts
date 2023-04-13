import {Component, OnDestroy, OnInit} from '@angular/core';
import {Title} from "@angular/platform-browser";
import {Subscription} from "rxjs";
import {ScrollServiceService} from "../../../shared/helpers/scroll-service.service";
import {HttpParamsServiceService} from "../../../shared/data-access/http-params-service.service";
import {PictureDto} from "../../../shared/utils/dtos/PictureDto";
import {AppCacheService} from "../../../shared/state/app-cache.service";
import {PictureDtoPaged} from "../../../shared/utils/dtos/PictureDtoPaged";
import {HomePageOption} from "../../../shared/utils/enums/homePageOption";
import {PictureService} from "../../../shared/data-access/picture/picture.service";

@Component({
  selector: 'app-body',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})

export class HomepageComponent implements OnInit, OnDestroy {
  items: (PictureDto)[] = [];
  isLoggedOn: boolean = false;
  pictureFetchingOption?: HomePageOption;

  private readonly subs = new Subscription();

  constructor(
    private pictureService: PictureService,
    private cacheService: AppCacheService,
    private scrollService: ScrollServiceService,
    private paramsService: HttpParamsServiceService,
    private title: Title
  ) {
    this.title.setTitle('PicturesUI - Strona główna');
  }

  ngOnInit() {
    this.isLoggedOn = this.cacheService.getUserLoggedOnState();
    this.pictureFetchingOption = this.isLoggedOn ? HomePageOption.PERSONALIZED : HomePageOption.MOST_POPULAR;
    this.items = this.cacheService.getCachedPictures();
    this.cacheService.purgeCachePictures();
    this.fetchPictures();

    this.subs.add(
      this.scrollService.bottomSubject
      .subscribe({
        next: (v: boolean) => {
          if (v) this.fetchPictures()
        }})
    );

  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  fetchPictures(): void {

    switch (this.pictureFetchingOption) {

      case HomePageOption.PERSONALIZED:
        this.subs.add(
          this.pictureService.getPersonalizedPictures(2)
            .subscribe({
              next: (value: PictureDtoPaged) => {
                if (value.items?.length !== 0 && value.items) {
                  value.items.forEach((pic: PictureDto) => this.items.push(pic)
                  );
                  this.scrollService.bottomSubject.next(false);
                  return;
                } else {
                  this.pictureFetchingOption = HomePageOption.MOST_POPULAR;
                  this.fetchPictures();
                }
              }
            })
        );
        break;

      case HomePageOption.MOST_POPULAR:
        this.subs.add(
          this.pictureService.getPictures(2, this.cacheService.mostPopularSite)
            .subscribe({
              next: (value: PictureDtoPaged) => {
                if (value.items?.length) {
                  value.items.forEach((pic: PictureDto) => {
                    this.items.push(pic);
                  });
                  this.cacheService.mostPopularSite = value.page+1;
                  this.scrollService.bottomSubject.next(false);
                } else {
                  this.cacheService.mostPopularSite = 1;
                  if (value.items?.length) this.fetchPictures();

                }
              }
            })
        );
        break;

    }
  }

}

