import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AccountDto} from "../../shared/utils/dtos/AccountDto";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(
    private httpClient: HttpClient
  )
  { }

  getAccountById(id: string): Observable<AccountDto>{
    return this.httpClient
      .get<AccountDto>(
        `${environment.picturesApiUrl}/api/account/${id}`
      );
  }

  deleteAccount(id: string): Observable<any> {
    return this.httpClient
      .delete(
        `${environment.picturesApiUrl}/api/account/${id}`
      );
  }

}
