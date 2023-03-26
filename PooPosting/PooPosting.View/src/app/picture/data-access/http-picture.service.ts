import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {PictureDto} from "../../shared/utils/dtos/PictureDto";
import {environment} from "../../../environments/environment";
import {PostPictureDto} from "../../shared/utils/dtos/PostPictureDto";

@Injectable({
  providedIn: 'root'
})
export class HttpPictureService {

  constructor(
    private httpClient: HttpClient
  ) { }
  postPicture(dto: FormData): Observable<any> {
    return this.httpClient
      .post(
        `${environment.picturesApiUrl}/api/picture/post`,
        dto
      );
  }


}
