import { Component, OnInit } from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import { HttpServiceService } from 'src/app/Services/http/http-service.service';
import { CustomValidators } from 'src/CustomValidators';
import {MessageService} from "primeng/api";
import {Router} from "@angular/router";
import {BlockSpace} from "../../../../Regexes/BlockSpace";
import {ItemName} from "../../../../Regexes/ItemName";
import {Title} from "@angular/platform-browser";
import {environment} from "../../../../../environments/environment";
import {error} from "@angular/compiler-cli/src/transformers/util";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  form!: UntypedFormGroup;
  blockSpace: RegExp = BlockSpace;
  isName: RegExp = ItemName;
  formDisabled: boolean = false;

  siteKey!: string;
  captchaPassed: boolean = false;
  isNickNameTaken: boolean = false;

  appUrl: string = "";

  passCaptcha() {
    this.captchaPassed = true;
  }

  constructor(
    private httpService: HttpServiceService,
    private message: MessageService,
    private router: Router,
    private title: Title,
  ) {
    this.siteKey = environment.captchaKey;
    this.title.setTitle(`PicturesUI - Rejestracja`);
  }

  ngOnInit(): void {
    this.appUrl = environment.appWebUrl;
    this.isNickNameTaken = false;
    this.form = new UntypedFormGroup({
      nickname: new UntypedFormControl(null, [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(16),
      ]),
      email: new UntypedFormControl(null, [
        Validators.required,
        Validators.minLength(10),
        Validators.email
      ]),
      password: new UntypedFormControl(null, [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(64),
      ]),
      confirmPassword: new UntypedFormControl(null, [
        Validators.required
      ])},
      //@ts-ignore
      CustomValidators.mustMatch('password', 'confirmPassword'),
    )
  }

  onSubmit(): void {
    this.disableForm();
    this.isNickNameTaken = false;
    this.message.clear();
    this.httpService.postRegisterRequest(this.form.getRawValue()).subscribe({
      next: () => {
        this.router.navigate(["/auth/login"]);
        this.message.add({severity:'success', summary: 'Sukces', detail: 'Zarejestrowano pomyślnie. Przeniesiono cię na stronę logowania.'});
      },
      error: (err) => {
        if (err.status === 200) {
          this.router.navigate(["/auth/login"]);
          this.message.add({severity:'success', summary: 'Sukces', detail: 'Zarejestrowano pomyślnie. Przeniesiono cię na stronę logowania.'});
          return
        }
        this.message.add({severity:'error', summary: 'Sukces', detail: 'Nie udało się zarejestrować. Sprawdź błędy.'});
        if (err.error.errors && err.error.errors.Nickname === "That nickname is taken") {
          this.isNickNameTaken = true;
        }
        this.enableForm();
      }
    });
  }

  openLink(url: string){
    window.open(url, "_blank");
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
