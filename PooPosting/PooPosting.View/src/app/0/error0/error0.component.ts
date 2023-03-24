
import { Component, OnInit } from '@angular/core';
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-error0',
  templateUrl: './error0.component.html',
  styleUrls: ['./error0.component.scss']
})
export class Error0Component implements OnInit {

  constructor(
    private title: Title
  ) { }

  ngOnInit(): void {
    this.title.setTitle('PicturesUI - Przerwa techniczna')
  }

}
