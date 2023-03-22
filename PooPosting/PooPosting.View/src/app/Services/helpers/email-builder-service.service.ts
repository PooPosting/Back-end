import {Injectable} from '@angular/core';
import {ErrorLogModel} from "../../Models/JsonModels/ErrorLogModel";
import {PostSendLogsModel} from "../../Models/ApiModels/Post/PostSendLogsModel";
import {CacheServiceService} from "../data/cache-service.service";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class EmailBuilderServiceService {

  constructor(
    private cacheService: CacheServiceService,
  ) {

  }

  buildEmail(errorLogs: ErrorLogModel, userMsg: string): PostSendLogsModel {
    let logs: PostSendLogsModel = {
      firstName: this.cacheService.getCachedUserAccount().nickname,
      emailAddress: this.cacheService.getCachedUserAccount().email,
      text: userMsg,
      sendingApp: environment.appWebUrl,
      jsonLogsAttachment: errorLogs.errors.length ? JSON.stringify(errorLogs, null, '\t') : ""
    };
    return logs;
  }
}
