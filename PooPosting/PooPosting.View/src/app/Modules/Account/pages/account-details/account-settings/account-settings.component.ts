import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {SelectOption} from "../../../../../Models/QueryModels/SelectOption";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpServiceService} from "../../../../../Services/http/http-service.service";
import {MessageService} from "primeng/api";
import {CustomValidators} from "../../../../../../CustomValidators";
import {ChangePasswordFormModel} from "../../../../../Models/FormModels/ChangePasswordFormModel";
import {ChangeAccountDescModel} from "../../../../../Models/FormModels/ChangeAccountDescModel";
import {ChangeEmailModel} from "../../../../../Models/FormModels/ChangeEmailModel";
import {concatWith} from "rxjs";
import {CropperComponent} from "angular-cropperjs";
import {AccountDto} from "../../../../../Models/Dtos/AccountDto";

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss']
})
export class AccountSettingsComponent {

  @Input() account!: AccountDto;
  selectOptions: SelectOption[];
  selectValue: SelectOption;

  blockSpace: RegExp = /[^\s]/;

  @Output() onChange: EventEmitter<AccountDto> = new EventEmitter<AccountDto>();

  changePass: FormGroup<ChangePasswordFormModel> = new FormGroup<ChangePasswordFormModel>({
    password: new FormControl("", [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(64),
    ]),
    confirmPassword: new FormControl("", [
        Validators.required,
      ]),},
    // @ts-ignore
    CustomValidators.mustMatch('password', 'confirmPassword'),
  );
  changeEmail: FormGroup<ChangeEmailModel> = new FormGroup<ChangeEmailModel>({
    email: new FormControl("", [
      Validators.required,
      Validators.email,
      Validators.maxLength(250)
    ]),
  });
  changeDesc: FormGroup<ChangeAccountDescModel> = new FormGroup<ChangeAccountDescModel>({
    description: new FormControl("", [
      Validators.required,
      Validators.maxLength(500)
    ])
  })

  awaitSubmit: boolean = false;

  constructor(
    private httpService: HttpServiceService,
    private messageService: MessageService,
  ) {
    this.selectOptions = [
      { name: "Zmiana hasła", class: "bi bi-lock" },
      { name: "Zmiana adresu e-mail", class: "bi bi-envelope" },
      { name: "Zmiana opisu powitalnego", class: "bi bi-card-text" },
      { name: "Zmiana obrazka profilowego", class: "bi bi-image" },
      { name: "Zmiana obrazka w tle", class: "bi bi-images" },
    ];
    this.selectValue = this.selectOptions[0];
    this.resetForms();
  }

  submitChangePass() {
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append('password', this.changePass.get('password')?.value)
    formData.append('confPassword', this.changePass.get('confirmPassword')?.value)
    this.httpService.updateAccountRequest(formData)
      .subscribe(this.updateAccountObserver);
  }
  submitChangeEmail() {
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append('email', this.changeEmail.get('email')?.value!)
    this.httpService.updateAccountRequest(formData)
      .subscribe(this.updateAccountObserver);
  }
  submitChangeDesc() {
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append('description', this.changeDesc.get('description')?.value!.replace(/[\n\r]/g, ''))
    this.httpService.updateAccountRequest(formData)
      .subscribe(this.updateAccountObserver);
  }
  submitChangeProfPic(dataUrl: Blob) {
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append('profilePic', dataUrl)
    this.httpService.updateAccountRequest(formData)
      .subscribe(this.updateAccountObserver);
  }
  submitChangeBgPic(dataUrl: Blob) {
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append('backgroundPic', dataUrl)
    this.httpService.updateAccountRequest(formData)
      .subscribe(this.updateAccountObserver);
  }

  updateAccountObserver = {
    next: (val: AccountDto) => {
      this.account.accountDescription = val.accountDescription;
      this.account.profilePicUrl = val.profilePicUrl;
      this.account.backgroundPicUrl = val.backgroundPicUrl;
      this.account.email = val.email;

      this.onChange.emit(val);
      this.messageService.add({
        severity: "success",
        summary: "Sukces",
        detail: "Pomyślnie zmieniono dane twojego konta!"
      });
      this.resetForms();
      this.awaitSubmit = false;
    },
    error: () => {
      this.awaitSubmit = false;
    }
  }

  resetForms() {
    this.changePass.reset();
    this.changeEmail.reset();
    this.changeDesc.reset();
  }

}
