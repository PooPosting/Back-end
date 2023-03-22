import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {map, Observable, Subscription} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpServiceService} from "../../../../Services/http/http-service.service";
import {AccountModel} from "../../../../Models/ApiModels/Get/AccountModel";
import {LocationServiceService} from "../../../../Services/helpers/location-service.service";
import {MessageService} from "primeng/api";
import {Title} from "@angular/platform-browser";
import {PictureDetailsServiceService} from "../../../../Services/data/picture-details-service.service";

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit, OnDestroy {
  id: Observable<string>;
  account!: AccountModel;
  showInfo: boolean = false;
  showShare: boolean = false;

  picDeletedSubscription: Subscription = new Subscription();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private httpService: HttpServiceService,
    private locationService: LocationServiceService,
    private pictureDetailsService: PictureDetailsServiceService,
    private title: Title
  ) {
    this.id = route.params.pipe(map(p => p['id']));

  }

  ngOnInit() {
    this.id.subscribe({
      next: (val) => {
        this.httpService.getAccountRequest(val)
          .subscribe(this.initialObserver);
      }
    });
    this.picDeletedSubscription = this.pictureDetailsService.pictureDeletedSubject.subscribe({
      next: (val) => {
        this.account.picturePreviews = this.account.picturePreviews.filter(p => p.id !== val);
      }
    });
  }
  ngOnDestroy() {
    this.picDeletedSubscription.unsubscribe();
  }

  return(): void {
    this.locationService.goBack();
  }

  updateAccount(account: AccountModel) {
    this.account.accountDescription = account.accountDescription;
    this.account.profilePicUrl = account.profilePicUrl;
    this.account.backgroundPicUrl = account.backgroundPicUrl;
    this.account.email = account.email;
  }

  banAccount() {
    this.httpService.deleteAccountRequest(this.account.id)
      .subscribe({
        next: (val) => {
          if(val) {
            this.messageService.add({
              severity: "warn",
              summary: `Sukces`,
              detail: `Konto ${this.account.nickname} zostało zbanowane.`
            });
            this.locationService.goBack();
          }
        },
      })
  }

  private initialObserver = {
    next: (acc: AccountModel) => {
      this.account = acc;
      this.title.setTitle(`PicturesUI - ${acc.nickname}`);
    },
    error: () => {
      this.router.navigate(['/error404']);
    }
  }

}
