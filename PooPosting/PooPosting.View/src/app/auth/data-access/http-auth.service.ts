import { Injectable } from '@angular/core';
import {LoginDto} from "../../shared/utils/dtos/LoginDto";
import {Observable} from "rxjs";
import {UserState} from "../../shared/utils/models/userState";
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {RegisterDto} from "../../shared/utils/dtos/RegisterDto";

@Injectable({
  providedIn: 'root'
})
export class HttpAuthService {

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

}
