import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {AccountDto} from "../../utils/dtos/AccountDto";
import {environment} from "../../../../environments/environment";
import {AccountDtoPaged} from "../../utils/dtos/AccountDtoPaged";
import {HttpParamsServiceService} from "../http-params-service.service";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    private httpClient: HttpClient,
    private paramsService: HttpParamsServiceService
  ) { }

  getAccountById(id: string): Observable<AccountDto>{
    return this.httpClient
      .get<AccountDto>(
        `${environment.picturesApiUrl}/api/account/${id}`,
        {responseType: "json",}
      );
  }

  searchAccounts(params: HttpParams): Observable<AccountDtoPaged>{
    return this.httpClient
      .get<AccountDtoPaged>(
        `${environment.picturesApiUrl}/api/account`,
        {
          params: params,
          responseType: "json"
        },
      );
  }

  deleteAccount(id: string): Observable<null> {
    return this.httpClient
      .delete<null>(
        `${environment.picturesApiUrl}/api/account/${id}`,
        {responseType: "json",}
      );
  }


}
