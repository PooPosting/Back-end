import {HttpErrorResponse, HttpRequest} from "@angular/common/http";

export interface ErrorInfoModel {
  date: string;
  status: string;
  error: HttpErrorResponse;
  request: HttpRequest<any>;
}
