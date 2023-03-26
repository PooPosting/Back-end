import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PutPostCommentDto} from "../../shared/utils/dtos/PutPostCommentDto";
import {Observable} from "rxjs";
import {CommentDto} from "../../shared/utils/dtos/CommentDto";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class HttpPopupService {
  constructor(
    private httpClient: HttpClient
  ) { }

  postComment(picId: string, data: PutPostCommentDto): Observable<CommentDto> {
    return this.httpClient
      .post<CommentDto>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment`,
        data
      );
  }
  deleteComment(picId: string, commId: string): Observable<CommentDto> {
    return this.httpClient
      .delete<CommentDto>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment/${commId}`,
        {}
      );
  }
  patchComment(commId: string, data: PutPostCommentDto): Observable<CommentDto> {
    return this.httpClient
      .patch<CommentDto>(
        `${environment.picturesApiUrl}/api/comment/${commId}`,
        data
      )
  }



}
