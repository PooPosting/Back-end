import {Component, OnInit} from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import {MessageService} from "primeng/api";
import {map, Observable, Subscription} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpServiceService} from "../../../../Services/http/http-service.service";
import {LocationServiceService} from "../../../../Services/helpers/location-service.service";
import {Title} from "@angular/platform-browser";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";
import {TitleCasePipe} from "@angular/common";
import {PictureDetailsServiceService} from "../../../../Services/data/picture-details-service.service";
import {PictureDto} from "../../../../Models/Dtos/PictureDto";

@Component({
  selector: 'app-picture-details',
  templateUrl: './picture-details.component.html',
  styleUrls: ['./picture-details.component.scss']
})
export class PictureDetailsComponent implements OnInit {
  picture!: PictureDto;
  id: Observable<string>;
  isLoggedOn: boolean = false;

  commentForm: UntypedFormGroup = new UntypedFormGroup({
    text: new UntypedFormControl("", [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(250)
    ])
  })
  awaitSubmit: boolean = false;

  showSettingsFlag: boolean = false;
  showAdminSettingsFlag: boolean = false;
  showShareFlag: boolean = false;

  picDeletedSubscription: Subscription = new Subscription();
  picChangedSubscription: Subscription = new Subscription();

  constructor(
    private cacheService: CacheServiceService,
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
    this.picDeletedSubscription = this.pictureDetailsService.pictureDeletedSubject.subscribe({
      next: (val: string) => {
        if (val === this.picture.id) {
          this.cacheService.cachedPictures = this.cacheService.getCachedPictures().filter(p => p.id !== this.picture.id);
          this.locationService.goBack();
        }
      }
    })
    this.picChangedSubscription = this.pictureDetailsService.pictureChangedSubject.subscribe({
      next: (val: PictureDto) => {
        if (val.id === this.picture.id) {
          this.picture = val;
        }
      }
    });
  }
  ngOnDestroy() {
    this.picDeletedSubscription.unsubscribe();
    this.picChangedSubscription.unsubscribe();
  }

  like() {
    this.httpService.patchPictureLikeRequest(this.picture.id)
      .subscribe(this.likeObserver)
  }
  dislike(){
    this.httpService.patchPictureDislikeRequest(this.picture.id)
      .subscribe(this.likeObserver)
  }

  showShare() {
    this.showShareFlag = true;
  }
  showDetails() {
    this.pictureDetailsService.modalTriggerSubject.next(this.picture.id);
  }
  return() {
    this.locationService.goBack();
  }

  initialSubscribe() {
    this.id.subscribe({
      next: (val) => {
        this.httpService.getPictureRequest(val).subscribe({
          next: (pic: PictureDto) => {
            this.picture = pic;
            this.title.setTitle(`PicturesUI - Obrazek "${this.titleCasePipe.transform(pic.name)}"`);
            this.cacheService.cachePictures([this.picture]);
          },
          error: () => {
            this.router.navigate(['/error404']);
          }
        });
      }
    })
  }

  likeObserver = {
    next: () => {
      this.httpService.getPictureRequest(this.picture.id).subscribe({
        next: (value: PictureDto) => {
          this.picture = value;
        }
      })
    },
    error: () => {
      this.messageService.add({
        severity:'error',
        summary: 'Niepowodzenie',
        detail: `Coś poszło nie tak.`
      });
    }
  }
}
