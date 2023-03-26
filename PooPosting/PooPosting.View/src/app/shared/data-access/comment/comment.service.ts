import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {PutPostCommentDto} from "../../utils/dtos/PutPostCommentDto";
import {Observable} from "rxjs";
import {CommentDto} from "../../utils/dtos/CommentDto";
import {environment} from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(
    private httpClient: HttpClient
  ) { }

  patchComment(picId: string, data: PutPostCommentDto): Observable<CommentDto> {
    return this.httpClient
      .patch<CommentDto>(
        `${environment.picturesApiUrl}/api/comment/${picId}`,
        data,
        { responseType: "json"}
      );
  }
  deleteComment(commId: string): Observable<null> {
    return this.httpClient
      .delete<null>(
        `${environment.picturesApiUrl}/api/comment/${commId}`,
        { responseType: "json"}
      );
  }

}
