import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ScrollServiceService {
  bottomSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  scrollTopState: number = 0;

  loadScrollState() {
    document.body.scrollTop = this.scrollTopState;
  }
  saveScrollState() {
    this.scrollTopState = document.body.scrollTop;
  }
  resetScrollState() {
    this.scrollTopState = 0;
  }
}
