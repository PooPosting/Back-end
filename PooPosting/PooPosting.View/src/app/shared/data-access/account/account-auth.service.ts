import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {LoginDto} from "../../utils/dtos/LoginDto";
import {Observable} from "rxjs";
import {UserState} from "../../utils/models/userState";
import {environment} from "../../../../environments/environment";
import {RegisterDto} from "../../utils/dtos/RegisterDto";
import {VerifyJwtDto} from "../../utils/dtos/VerifyJwtDto";

@Injectable({
  providedIn: 'root'
})
export class AccountAuthService {

  constructor(
    private httpClient: HttpClient
  ) { }

  login(dto: LoginDto): Observable<UserState> {
    return this.httpClient
      .post<UserState>(
        `${environment.picturesApiUrl}/api/account/auth/login`,
        dto,
        {responseType: "json",});
  }

  register(data: RegisterDto): Observable<any> {
    return this.httpClient
      .post(
        `${environment.picturesApiUrl}/api/account/auth/register`,
        data,
        {responseType: "json",});
  }

  verifyJwt(data: VerifyJwtDto): Observable<UserState> {
    return this.httpClient
      .post<UserState>(
        `${environment.picturesApiUrl}/api/account/auth/verifyJwt`,
        data,
        {responseType: "json",});
  }

}
