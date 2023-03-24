import {Component, OnInit} from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import { HttpServiceService } from 'src/app/shared/data-access/http-service.service';
import {MessageService} from "primeng/api";
import {UserState} from "../../../shared/utils/models/userState";
import {LocationServiceService} from "../../../shared/helpers/location-service.service";
import {Title} from "@angular/platform-browser";
import {AppCacheService} from "../../../shared/state/app-cache.service";
import {AuthService} from "../../data-access/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form!: UntypedFormGroup;
  formDisabled: boolean = false;

  constructor(
    private cacheService: AppCacheService,
    private locationService: LocationServiceService,
    private authService: AuthService,
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
  }

  private disableForm() {
    this.form.disable();
    this.formDisabled = true;
  }
  private enableForm() {
    this.form.enable();
    this.formDisabled = false;
  }

}
