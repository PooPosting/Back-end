import { Injectable } from '@angular/core';
import {UpdateAccountPasswordDto} from "../../utils/dtos/UpdateAccountPasswordDto";
import {AccountDto} from "../../utils/dtos/AccountDto";
import {environment} from "../../../../environments/environment";
import {UpdateAccountDescriptionDto} from "../../utils/dtos/UpdateAccountDescriptionDto";
import {HttpClient} from "@angular/common/http";
import {UpdateAccountEmailDto} from "../../utils/dtos/UpdateAccountEmailDto";

@Injectable({
  providedIn: 'root'
})
export class AccountUpdateService {

  constructor(
    private httpClient: HttpClient
  ) { }

  updateAccountEmail(data: UpdateAccountEmailDto) {
    return this.httpClient
      .post<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/email`,
        data,
        { responseType: "json" }
      );
  }

  updateAccountPassword(data: UpdateAccountPasswordDto) {
    return this.httpClient
      .post<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/password`,
        data,
        { responseType: "json" }
      );
  }

  updateAccountDescription(data: UpdateAccountDescriptionDto) {
    return this.httpClient
      .patch<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/description`,
        data,
        { responseType: "json" }
      );
  }

  updateAccountProfilePicture(file: string) {
    return this.httpClient
      .patch<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/profile-picture`,
        file,
        { responseType: "json" }
      );
  }

  updateAccountBackgroundPicture(file: string) {
    return this.httpClient
      .patch<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/background-picture`,
        file,
        { responseType: "json" }
      );
  }
}
