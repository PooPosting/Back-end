import { Component, OnInit } from '@angular/core';
import {PopularModel} from "../../../../Models/ApiModels/Get/PopularModel";
import {HttpServiceService} from "../../../../Services/http/http-service.service";
import {SelectOption} from "../../../../Models/QueryModels/SelectOption";
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-popular',
  templateUrl: './popular.component.html',
  styleUrls: ['./popular.component.scss']
})
export class PopularComponent implements OnInit {
  popular: PopularModel = {
    mostCommentedPictures: [],
    mostLikedPictures: [],
    mostVotedPictures: [],
    mostLikedAccounts: [],
    mostPostedAccounts: [],
  };
  selectOptions: SelectOption[];
  selectValue: SelectOption;
  accountOptions: SelectOption[];
  accountValue: SelectOption;
  pictureOptions: SelectOption[];
  pictureValue: SelectOption;

  responsiveOptions = [
    {
      breakpoint: '560px',
      numVisible: 1,
      numScroll: 1
    }
  ];

  constructor(
    private httpService: HttpServiceService,
    private title: Title
  ) {
    this.title.setTitle('PicturesUI - Popularne');
    this.selectOptions = [
      { name: "Obrazki", class: "bi bi-image-fill"},
      { name: "Konta", class: "bi bi-person-fill"},
    ];
    this.selectValue = this.selectOptions[0];
    this.accountOptions = [
      { name: "Lajków", class: "bi bi-hand-thumbs-up"},
      { name: "Postów", class: "bi bi-graph-up-arrow"},
    ]
    this.accountValue = this.accountOptions[0];
    this.pictureOptions = [
      { name: "Komentowane", class: "bi bi-chat-right-text"},
      { name: "Lajkowane", class: "bi bi-hand-thumbs-up"},
      { name: "Oceniane", class: "bi bi-star"},
    ]
    this.pictureValue = this.pictureOptions[1];
  }

  ngOnInit(): void {
    this.httpService.getPopularRequest().subscribe({
      next: (val: PopularModel) => {
        this.popular = val;
      }
    });
  }

}
