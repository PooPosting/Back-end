import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {PictureDto} from "../../utils/dtos/PictureDto";
import {environment} from "../../../../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {LikeDtoPaged} from "../../utils/dtos/LikeDtoPaged";

@Injectable({
  providedIn: 'root'
})
export class PictureLikesService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getPictureLikes(id: string, pageSize: number, pageNumber: number): Observable<LikeDtoPaged> {
    return this.httpClient
      .get<LikeDtoPaged>(
        `${environment.picturesApiUrl}/api/picture/${id}/like`,
        {
          params: new HttpParams()
            .set("PageSize", pageSize)
            .set("PageNumber", pageNumber),
          responseType: "json"
        }
      );
  }

  likePicture(id: string): Observable<PictureDto> {
    return this.httpClient
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/like/vote-up`,
        {},
        { responseType: "json" }
      );
  }
  dislikePicture(id: string): Observable<PictureDto> {
    return this.httpClient
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/like/vote-down`,
        {},
        { responseType: "json" }
      );
  }
}
