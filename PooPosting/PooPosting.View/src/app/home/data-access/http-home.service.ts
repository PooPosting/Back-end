import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {PictureDtoPaged} from "../../shared/utils/dtos/PictureDtoPaged";
import {environment} from "../../../environments/environment";
import {PictureDto} from "../../shared/utils/dtos/PictureDto";
import {HttpParamsServiceService} from "../../shared/data-access/http-params-service.service";

@Injectable({
  providedIn: 'root'
})
export class HttpHomeService {

  constructor(
    private httpClient: HttpClient,
    private paramsService: HttpParamsServiceService
  ) { }

  getTrendingPictures(params: HttpParams): Observable<PictureDtoPaged>{
    return this.httpClient
      .get<PictureDtoPaged>(
        `${environment.picturesApiUrl}/api/picture`,
        {params: params}
      );
  }
  getPersonalizedPictures(): Observable<PictureDto[]>{
    return this.httpClient
      .get<PictureDto[]>(
        `${environment.picturesApiUrl}/api/picture/personalized`,
        {params: this.paramsService.getGetPersonalizedPicParams()}
      );
  }

}
