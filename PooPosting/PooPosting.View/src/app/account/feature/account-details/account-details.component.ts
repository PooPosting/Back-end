import {HttpServiceService} from "../../../shared/data-access/http-service.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Title} from "@angular/platform-browser";
import {map, Observable, Subscription} from "rxjs";
import {MessageService} from "primeng/api";
import {Component, OnDestroy, OnInit} from "@angular/core";
import {PictureDetailsServiceService} from "../../../shared/state/picture-details-service.service";
import {LocationServiceService} from "../../../shared/helpers/location-service.service";
import {AccountDto} from "../../../shared/utils/dtos/AccountDto";
import {HttpAccountService} from "../../data-access/http-account.service";

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit, OnDestroy {
  id: Observable<string>;
  account!: AccountDto;
  showInfo: boolean = false;
  showShare: boolean = false;

  picDeletedSubscription: Subscription = new Subscription();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private accountService: HttpAccountService,
    private messageService: MessageService,
    private locationService: LocationServiceService,
    private pictureDetailsService: PictureDetailsServiceService,
    private title: Title
  ) {
    this.id = route.params.pipe(map(p => p['id']));
  }

  ngOnInit() {
    this.id.subscribe({
      next: (id) => {
        this.accountService.getAccountById(id)
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

  updateAccount(account: AccountDto) {
    this.account.accountDescription = account.accountDescription;
    this.account.profilePicUrl = account.profilePicUrl;
    this.account.backgroundPicUrl = account.backgroundPicUrl;
    this.account.email = account.email;
  }

  banAccount() {
    this.accountService.deleteAccount(this.account.id)
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
    next: (acc: AccountDto) => {
      this.account = acc;
      this.title.setTitle(`PicturesUI - ${acc.nickname}`);
    },
    error: () => {
      this.router.navigate(['/error404']);
    }
  }

}
