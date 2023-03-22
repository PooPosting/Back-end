import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {HttpParamsServiceService} from "./http-params-service.service";
import { PictureDto } from 'src/app/Models/Dtos/PictureDto';
import { PictureDtoPaged } from 'src/app/Models/Dtos/PictureDtoPaged';
import { LoginDto } from 'src/app/Models/Dtos/LoginDto';
import { UserInfoModel } from 'src/app/Models/UserInfoModel';
import { RegisterDto } from 'src/app/Models/Dtos/RegisterDto';
import {LikeDto} from "../../Models/Dtos/LikeDto";
import {AccountDtoPaged} from "../../Models/Dtos/AccountDtoPaged";
import {AccountDto} from "../../Models/Dtos/AccountDto";
import { CommentDto } from "../../Models/Dtos/CommentDto";
import {PutPostCommentDto} from "../../Models/Dtos/PutPostCommentDto";
import {PopularDto} from "../../Models/Dtos/PopularDto";
import {VerifyJwtDto} from "../../Models/Dtos/VerifyJwtDto";
import {PostLogsDto} from "../../Models/Dtos/PostLogsDto";
import {PictureClassificationDto} from "../../Models/Dtos/PictureClassificationDto";
import {environment} from "../../../environments/environment";
import {UpdatePictureNameDto} from "../../Models/Dtos/UpdatePictureNameDto";
import {UpdatePictureDescriptionDto} from "../../Models/Dtos/UpdatePictureDescriptionDto";
import {UpdatePictureTagsDto} from "../../Models/Dtos/UpdatePictureTagsDto";
import {UpdateAccountEmailDto} from "../../Models/Dtos/UpdateAccountEmailDto";
import {UpdateAccountPasswordDto} from "../../Models/Dtos/UpdateAccountPasswordDto";
import {UpdateAccountDescriptionDto} from "../../Models/Dtos/UpdateAccountDescriptionDto";

@Injectable({
  providedIn: 'root'
})
export class HttpServiceService {
  constructor(
    private http: HttpClient,
    private params: HttpParamsServiceService
  ) {}

  getPicturesRequest(params: HttpParams): Observable<PictureDtoPaged>{
    return this.http
      .get<PictureDtoPaged>(
      `${environment.picturesApiUrl}/api/picture`,
      {params: params}
    );
  }
  getPersonalizedPicturesRequest(): Observable<PictureDto[]>{
    return this.http
      .get<PictureDto[]>(
        `${environment.picturesApiUrl}/api/picture/personalized`,
        {params: this.params.getGetPersonalizedPicParams()}
      );
  }
  searchPicturesRequest(): Observable<PictureDtoPaged>{
    return this.http
      .get<PictureDtoPaged>(
        `${environment.picturesApiUrl}/api/picture/search`,
        {params: this.params.getSearchPicParams()}
      );
  }

  getPictureRequest(id?: string): Observable<PictureDto>{
    return this.http
      .get<PictureDto>(
      `${environment.picturesApiUrl}/api/picture/${id}`
    );
  }
  getPictureLikesRequest(id?: string): Observable<LikeDto[]>{
    return this.http
      .get<LikeDto[]>(
        `${environment.picturesApiUrl}/api/picture/${id}/like`
      );
  }
  searchAccountsRequest(): Observable<AccountDtoPaged>{
    return this.http
      .get<AccountDtoPaged>(
        `${environment.picturesApiUrl}/api/account`,
        {params: this.params.getSearchAccParams()}
      );
  }
  getAccountRequest(id: string): Observable<AccountDto>{
    return this.http
      .get<AccountDto>(
        `${environment.picturesApiUrl}/api/account/${id}`
      );
  }
  getAccountLikesRequest(id?: string): Observable<LikeDto[]>{
    return this.http
      .get<LikeDto[]>(
        `${environment.picturesApiUrl}/api/account/${id}/likes`
      );
  }
  getPopularRequest(): Observable<PopularDto> {
    return this.http
      .get<PopularDto>(
        `${environment.picturesApiUrl}/api/popular`
      );
  }
  postLoginRequest(data: LoginDto): Observable<UserInfoModel> {
    return this.http
      .post<UserInfoModel>(
        `${environment.picturesApiUrl}/api/account/auth/login`,
        data,
        {responseType: "json",});
  }
  postLsLoginRequest(data: VerifyJwtDto): Observable<UserInfoModel> {
    return this.http
      .post<UserInfoModel>(
        `${environment.picturesApiUrl}/api/account/auth/verifyJwt`,
        data,
        {responseType: "json",});
  }
  postRegisterRequest(data: RegisterDto): Observable<any> {
    return this.http
      .post(
        `${environment.picturesApiUrl}/api/account/auth/register`,
        data,
        {responseType: "json",});
  }
  postPictureRequest(data: FormData): Observable<any> {
    return this.http
      .post(
        `${environment.picturesApiUrl}/api/picture/post`,
        data
      );
  }
  postClassifyPictureRequest(data: FormData): Observable<PictureClassificationDto> {
    return this.http
      .post<PictureClassificationDto>(
        `${environment.picturesApiUrl}/api/picture/classify`,
        data
      );
  }
  postCommentRequest(picId: string, data: PutPostCommentDto): Observable<CommentDto> {
    return this.http
      .post<CommentDto>(
        `${environment.picturesApiUrl}/api/picture/${picId}/comment`,
        data
      );
  }

  postSendLogsRequest(data: PostLogsDto) {
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


  deleteCommentRequest(picId: string, commId: string): Observable<CommentDto> {
    return this.http
      .delete<CommentDto>(
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

  patchPictureLikeRequest(id: string): Observable<PictureDto> {
    return this.http
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/vote-up`,
        {}
      );
  }
  patchPictureDislikeRequest(id: string): Observable<PictureDto> {
    return this.http
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/vote-down`,
        {}
      );
  }

  updatePictureRequest(data: FormData, id: string) {
    return this.http
      .post<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}`,
        data
      );
  }

  updatePictureNameRequest(data: UpdatePictureNameDto, id: string) {
    return this.http
      .post<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/update/name`,
        data
      );
  }

  updatePictureDescriptionRequest(data: UpdatePictureDescriptionDto, id: string) {
    return this.http
      .post<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/update/description`,
        data
      );
  }

  updatePictureTagsRequest(data: UpdatePictureTagsDto, id: string) {
    return this.http
      .post<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/update/tags`,
        data
      );
  }

  updateAccountRequest(data: FormData) {
    return this.http
      .post<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update`,
        data
      );
  }

  updateAccountEmailRequest(data: UpdateAccountEmailDto) {
    return this.http
      .post<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/email`,
        data
      );
  }

  updateAccountPasswordRequest(data: UpdateAccountPasswordDto) {
    return this.http
      .post<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/password`,
        data
      );
  }

  updateAccountDescriptionRequest(data: UpdateAccountDescriptionDto) {
    return this.http
      .patch<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/description`,
        data
      );
  }

  updateAccountProfilePictureRequest(file: string) {
    return this.http
      .patch<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/profile-picture`,
        file
      );
  }

  updateAccountBackgroundPictureRequest(file: string) {
    return this.http
      .patch<AccountDto>(
        `${environment.picturesApiUrl}/api/account/update/background-picture`,
        file
      );
  }

}
