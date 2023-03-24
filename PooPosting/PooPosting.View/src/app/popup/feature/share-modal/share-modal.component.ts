import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import {MessageService} from "primeng/api";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-share-modal',
  templateUrl: './share-modal.component.html',
  styleUrls: ['./share-modal.component.scss']
})
export class ShareModalComponent implements OnInit {
  @Output() onCopy: EventEmitter<any> = new EventEmitter<any>();
  @Input() id!: string;
  @Input() itemType!: string;
  url: string = "";

  constructor(
    private messageService: MessageService,
    private clipboard: Clipboard,
  ) { }

  ngOnInit() {
    this.url = `${environment.appWebUrl}/${this.itemType}/${this.id}`;
  }

  copyUrl(textToCopy: string) {
    this.messageService.clear();
    this.clipboard.copy(textToCopy);
    this.onCopy.emit();
    this.messageService.add({
      severity: 'success',
      summary: 'Sukces!',
      detail: 'Pomyślnie skopiowano adres obrazka!',
    })
  }

}
