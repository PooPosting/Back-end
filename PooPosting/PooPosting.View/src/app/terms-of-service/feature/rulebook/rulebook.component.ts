import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-rulebook',
  templateUrl: './rulebook.component.html',
  styleUrls: ['./rulebook.component.scss']
})
export class RulebookComponent implements OnInit {

  constructor(
    private router: Router,
    private title: Title
  ) {
    this.title.setTitle(`PicturesUI - Regulamin`);
  }

  ngOnInit(): void {
  }

}
