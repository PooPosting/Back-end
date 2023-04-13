import {Component, OnDestroy, OnInit} from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import {MessageService} from "primeng/api";
import {UserState} from "../../../shared/utils/models/userState";
import {LocationServiceService} from "../../../shared/helpers/location-service.service";
import {Title} from "@angular/platform-browser";
import {AppCacheService} from "../../../shared/state/app-cache.service";
import {AccountAuthService} from "../../../shared/data-access/account/account-auth.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  form!: UntypedFormGroup;
  formDisabled: boolean = false;

  private readonly subs = new Subscription();

  constructor(
    private cacheService: AppCacheService,
    private locationService: LocationServiceService,
    private authService: AccountAuthService,
    private messageService: MessageService,
    private title: Title
  ) {
    this.title.setTitle(`PicturesUI - Logowanie`);
  }

  ngOnInit(): void {
    this.form = new UntypedFormGroup({
      nickname: new UntypedFormControl(null, Validators.required),
      password: new UntypedFormControl(null, Validators.required),
    });
  }

  onSubmit(): void {
    this.messageService.clear();
    this.disableForm();

    this.subs.add(
      this.authService
        .login(this.form.getRawValue())
        .subscribe({
          next: (v: UserState) => {
            if (v) {
              this.cacheService.cacheUserInfo(v);
              this.messageService.add({severity:'success', summary: 'Sukces', detail: 'Zalogowano pomyślnie.'});
              this.cacheService.loggedOnSubject.next(true);
              this.cacheService.updateUserAccount();
              this.locationService.goBack();
            }
          },
          error: (err) => {
            this.messageService.add(
              {
                severity:'error',
                summary: 'Niepowodzenie',
                detail: err.error,
                key: "login-failed"}
            );
            this.enableForm();
          }
        })
    );

  }

  private disableForm() {
    this.form.disable();
    this.formDisabled = true;
  }
  private enableForm() {
    this.form.enable();
    this.formDisabled = false;
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

}