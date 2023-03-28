import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {PutPostCommentDto} from "../../utils/dtos/PutPostCommentDto";
import {Observable} from "rxjs";
import {CommentDto} from "../../utils/dtos/CommentDto";
import {environment} from "../../../../environments/environment";
import {CommentDtoPaged} from "../../utils/dtos/CommentDtoPaged";

@Injectable({
  providedIn: 'root'
})
export class PictureCommentsService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getPictureComments(picId: string, pageSize: number, pageNumber: number): Observable<CommentDtoPaged> {
    return this.httpClient
      .get<CommentDtoPaged>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment`,
        {
          params: new HttpParams()
            .set("PageSize", pageSize)
            .set("PageNumber", pageNumber),
          responseType: "json"
        },
      );
  }
  postComment(picId: string, data: PutPostCommentDto): Observable<CommentDto> {
    return this.httpClient
      .post<CommentDto>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment`,
        data,
        { responseType: "json" },
      );
  }

}
