import { Component, OnInit } from '@angular/core';
import {SessionStorageServiceService} from "../../../../Services/data/session-storage-service.service";
import {ErrorLogModel} from "../../../../Models/JsonModels/ErrorLogModel";
import {ErrorInfoModel} from "../../../../Models/JsonModels/ErrorInfoModel";
import {Clipboard} from "@angular/cdk/clipboard";
import {MessageService} from "primeng/api";
import {EmailBuilderServiceService} from "../../../../Services/helpers/email-builder-service.service";
import {HttpServiceService} from "../../../../Services/http/http-service.service";
import {Title} from "@angular/platform-browser";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.scss']
})
export class LogsComponent implements OnInit {
  logs!: ErrorLogModel;
  displayLogs: string[] = [];
  canSendLogs?: boolean = undefined;
  isLoggedOn: boolean = false;

  userMsg: string = "";

  constructor(
    private clipboard: Clipboard,
    private messageService: MessageService,
    private sessionStorageService: SessionStorageServiceService,
    private cacheService: CacheServiceService,
    private emailBuilderService: EmailBuilderServiceService,
    private httpService: HttpServiceService,
    private title: Title
  ) {
    this.title.setTitle(`PicturesUI - Zgłoś błąd`);
  }

  ngOnInit(): void {
    this.logs = this.sessionStorageService.getLogs();
    this.isLoggedOn = this.cacheService.getUserLoggedOnState();
    if(this.logs) {
      this.logs.errors.forEach((err) => {
        this.displayLogs.push(JSON.stringify(err, null, '\t'));
      })
    }
    this.httpService.postCheckEmailSendingAvailability()
      .subscribe(val => {
        this.canSendLogs = val;
      })
  }

  copyErrorLog(error: ErrorInfoModel) {
    this.messageService.clear();
    this.clipboard.copy(this.jsonToString(error));
    this.messageService.add({
      severity: 'success',
      summary: 'Sukces!',
      detail: `Pomyślnie skopiowano log błędu z ${error.date} do schowka!`,
    })
  }

  sendErrorLogs() {
    this.httpService.postSendLogsRequest(this.emailBuilderService.buildEmail(this.logs, this.userMsg))
    .subscribe({
      next: (val: boolean) => {
        if (val) {
          this.canSendLogs = false;
          this.messageService.add({
            severity: 'success',
            summary: 'Sukces!',
            detail: `Pomyślnie wysłano wiadomość oraz error logi do programisty!`,
          })
          this.userMsg = "";
        }
      }
    })
  }

  stringToJson(val: string) {
    return JSON.parse(val);
  }
  jsonToString(val: ErrorInfoModel) {
    return JSON.stringify(val, null, '\t');
  }
  stringToBool(val: string | undefined | null) {
    return val == "true";
  }

  parseDate(val: string) {
    let hms = val.split(',')[1];
    let h = hms.split(":")[0];
    let m = hms.split(":")[1];
    let s = hms.split(":")[2];
    let timeParsed = `${h.length > 1 ? h : "0" + h}:${m.length > 1 ? m : "0" + h}:${s.length > 1 ? s : "0" + s}`;
    return timeParsed;
  }

}
