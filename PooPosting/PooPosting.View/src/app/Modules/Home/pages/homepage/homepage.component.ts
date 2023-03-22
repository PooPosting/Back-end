import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpServiceService} from 'src/app/Services/http/http-service.service';
import {ScrollServiceService} from "../../../../Services/helpers/scroll-service.service";
import {Title} from "@angular/platform-browser";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";
import {HomePageOption} from "../../../../Enums/HomePageOption";
import {HttpParamsServiceService} from "../../../../Services/http/http-params-service.service";
import {Subscription} from "rxjs";
import {PictureDto} from "../../../../Models/Dtos/PictureDto";
import {PictureDtoPaged} from "../../../../Models/Dtos/PictureDtoPaged";

@Component({
  selector: 'app-body',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})

export class HomepageComponent implements OnInit, OnDestroy {
  items: (PictureDto)[] = [];
  isLoggedOn: boolean = false;
  pictureFetchingOption?: HomePageOption;

  scrollSubscription!: Subscription;

  constructor(
    private httpService: HttpServiceService,
    private cacheService: CacheServiceService,
    private scrollService: ScrollServiceService,
    private paramsService: HttpParamsServiceService,
    private title: Title
  ) {
    this.title.setTitle('PicturesUI - Strona główna');
  }

  ngOnInit() {
    this.isLoggedOn = this.cacheService.getUserLoggedOnState();
    this.pictureFetchingOption = this.isLoggedOn ? HomePageOption.PERSONALIZED : HomePageOption.MOST_POPULAR;

    this.scrollSubscription = this.scrollService.bottomSubject
      .subscribe({
        next: (v: boolean) => {
          if (v) {
            this.fetchPictures();
          }
        },
      });
    this.items = this.cacheService.getCachedPictures();
    this.cacheService.purgeCachePictures();
    this.fetchPictures();
  }

  ngOnDestroy(): void {
    this.scrollSubscription.unsubscribe();
  }

  fetchPictures(): void {

    switch (this.pictureFetchingOption) {

      case HomePageOption.PERSONALIZED: {
        this.httpService.getPersonalizedPicturesRequest()
          .subscribe({
          next: (value: PictureDto[]) => {
            if (value.length !== 0) {
              let loadedItems: PictureDto[] = this.items;
              value.forEach((pic: PictureDto) => {
                loadedItems.push(pic);
              });
              this.items = loadedItems;
              this.scrollService.bottomSubject.next(false);
              return;
            } else {
              this.pictureFetchingOption = HomePageOption.MOST_POPULAR;
              this.fetchPictures();
            }
          }
        });
        break;
      }

      case HomePageOption.MOST_POPULAR: {
        this.httpService.getPicturesRequest(
          this.paramsService.getGetPicParams(this.cacheService.mostPopularSite)
        ).subscribe({
          next: (value: PictureDtoPaged) => {
            if (value.items.length) {
              let loadedItems: PictureDto[] = this.items;
              value.items.forEach((pic: PictureDto) => {
                loadedItems.push(pic);
              });
              this.items = loadedItems;
              this.cacheService.mostPopularSite = value.page+1;
              this.scrollService.bottomSubject.next(false);
            } else {
              this.cacheService.mostPopularSite = 1;
              this.fetchPictures();
            }
          }
        });
        break;
      }

    }
  }

}

