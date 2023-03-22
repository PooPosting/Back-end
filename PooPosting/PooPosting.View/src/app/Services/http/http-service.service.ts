import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {HttpParamsServiceService} from "./http-params-service.service";
import { PictureModel } from 'src/app/Models/ApiModels/Get/PictureModel';
import { PicturePagedResult } from 'src/app/Models/ApiModels/Get/PicturePagedResult';
import { LoginModel } from 'src/app/Models/ApiModels/Post/LoginModel';
import { UserInfoModel } from 'src/app/Models/UserInfoModel';
import { RegisterModel } from 'src/app/Models/ApiModels/Post/RegisterModel';
import {LikeModel} from "../../Models/ApiModels/Get/LikeModel";
import {AccountPagedResult} from "../../Models/ApiModels/Get/AccountPagedResult";
import {AccountModel} from "../../Models/ApiModels/Get/AccountModel";
import {CommentModel} from "../../Models/ApiModels/Get/CommentModel";
import {PutPostCommentModel} from "../../Models/ApiModels/Post/PutPostCommentModel";
import {PopularModel} from "../../Models/ApiModels/Get/PopularModel";
import {LsJwtDetails} from "../../Models/ApiModels/Post/LsJwtDetails";
import {PostSendLogsModel} from "../../Models/ApiModels/Post/PostSendLogsModel";
import {PictureClassifiedModel} from "../../Models/ApiModels/Post/PictureClassifiedModel";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class HttpServiceService {
  constructor(
    private http: HttpClient,
    private params: HttpParamsServiceService
  ) {}

  getPicturesRequest(params: HttpParams): Observable<PicturePagedResult>{
    return this.http
      .get<PicturePagedResult>(
      `${environment.picturesApiUrl}/api/picture`,
      {params: params}
    );
  }
  getPersonalizedPicturesRequest(): Observable<PictureModel[]>{
    return this.http
      .get<PictureModel[]>(
        `${environment.picturesApiUrl}/api/picture/personalized`,
        {params: this.params.getGetPersonalizedPicParams()}
      );
  }
  searchPicturesRequest(): Observable<PicturePagedResult>{
    return this.http
      .get<PicturePagedResult>(
        `${environment.picturesApiUrl}/api/picture/search`,
        {params: this.params.getSearchPicParams()}
      );
  }

  getPictureRequest(id?: string): Observable<PictureModel>{
    return this.http
      .get<PictureModel>(
      `${environment.picturesApiUrl}/api/picture/${id}`
    );
  }
  getPictureLikesRequest(id?: string): Observable<LikeModel[]>{
    return this.http
      .get<LikeModel[]>(
        `${environment.picturesApiUrl}/api/picture/${id}/likes`
      );
  }
  searchAccountsRequest(): Observable<AccountPagedResult>{
    return this.http
      .get<AccountPagedResult>(
        `${environment.picturesApiUrl}/api/account`,
        {params: this.params.getSearchAccParams()}
      );
  }
  getAccountRequest(id: string): Observable<AccountModel>{
    return this.http
      .get<AccountModel>(
        `${environment.picturesApiUrl}/api/account/${id}`
      );
  }
  getAccountLikesRequest(id?: string): Observable<LikeModel[]>{
    return this.http
      .get<LikeModel[]>(
        `${environment.picturesApiUrl}/api/account/${id}/likes`
      );
  }
  getPopularRequest(): Observable<PopularModel> {
    return this.http
      .get<PopularModel>(
        `${environment.picturesApiUrl}/api/popular`
      );
  }
  postLoginRequest(data: LoginModel): Observable<UserInfoModel> {
    return this.http
      .post<UserInfoModel>(
        `${environment.picturesApiUrl}/api/account/auth/login`,
        data,
        {responseType: "json",});
  }
  postLsLoginRequest(data: LsJwtDetails): Observable<UserInfoModel> {
    return this.http
      .post<UserInfoModel>(
        `${environment.picturesApiUrl}/api/account/auth/verifyJwt`,
        data,
        {responseType: "json",});
  }
  postRegisterRequest(data: RegisterModel): Observable<any> {
    return this.http
      .post(
        `${environment.picturesApiUrl}/api/account/auth/register`,
        data,
        {responseType: "json",});
  }
  postPictureRequest(data: FormData): Observable<any> {
    return this.http
      .post(
        `${environment.picturesApiUrl}/api/picture/create`,
        data
      );
  }
  postClassifyPictureRequest(data: FormData): Observable<PictureClassifiedModel> {
    return this.http
      .post<PictureClassifiedModel>(
        `${environment.picturesApiUrl}/api/picture/classify`,
        data
      );
  }
  postCommentRequest(picId: string, data: PutPostCommentModel): Observable<CommentModel> {
    return this.http
      .post<CommentModel>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment`,
        data
      );
  }

  postSendLogsRequest(data: PostSendLogsModel) {
    return this.http
      .post<boolean>(
        `${environment.emailApiUrl}/api/contact/sendErrorEmail`,
        data
      )
  }
  postCheckEmailSendingAvailability() {
    return this.http
      .post<boolean>(
        `${environment.emailApiUrl}/api/contact/check`,
        {}
      )
  }


  deleteCommentRequest(picId: string, commId: string): Observable<CommentModel> {
    return this.http
      .delete<CommentModel>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment/${commId}`,
        {}
      );
  }
  deletePictureRequest(id: string): Observable<any> {
    return this.http
      .delete(
        `${environment.picturesApiUrl}/api/picture/${id}`
      );
  }
  deleteAccountRequest(id: string): Observable<any> {
    return this.http
      .delete(
        `${environment.picturesApiUrl}/api/account/${id}`
      );
  }

  patchPictureLikeRequest(id: string): Observable<PictureModel> {
    return this.http
      .patch<PictureModel>(
        `${environment.picturesApiUrl}/api/picture/${id}/voteup`,
        {}
      );
  }
  patchPictureDislikeRequest(id: string): Observable<PictureModel> {
    return this.http
      .patch<PictureModel>(
        `${environment.picturesApiUrl}/api/picture/${id}/votedown`,
        {}
      );
  }

  updatePictureRequest(data: FormData, id: string) {
    return this.http
      .post<PictureModel>(
        `${environment.picturesApiUrl}/api/picture/${id}`,
        data
      );
  }
  updateAccountRequest(data: FormData) {
    return this.http
      .post<AccountModel>(
        `${environment.picturesApiUrl}/api/account/update`,
        data
      );
  }


}
