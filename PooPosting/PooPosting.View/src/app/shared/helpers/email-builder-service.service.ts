import {Injectable} from '@angular/core';
import {ErrorLogModel} from "../../report/utils/errorLogModel";
import {PostLogsDto} from "../utils/dtos/PostLogsDto";
import {AppCacheService} from "../state/app-cache.service";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class EmailBuilderServiceService {

  constructor(
    private cacheService: AppCacheService,
  ) {

  }

  buildEmail(errorLogs: ErrorLogModel, userMsg: string): PostLogsDto {
    let logs: PostLogsDto = {
      firstName: this.cacheService.getCachedUserAccount().nickname,
      emailAddress: this.cacheService.getCachedUserAccount().email,
      text: userMsg,
      sendingApp: environment.appWebUrl,
      jsonLogsAttachment: errorLogs.errors.length ? JSON.stringify(errorLogs, null, '\t') : ""
    };
    return logs;
  }
}
