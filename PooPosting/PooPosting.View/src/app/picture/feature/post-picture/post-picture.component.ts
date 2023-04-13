import {Component} from '@angular/core';
import {MenuItem} from "primeng/api";
import {Router} from "@angular/router";

@Component({
  selector: 'app-post-picture',
  templateUrl: './post-picture.component.html',
  styleUrls: ['./post-picture.component.scss'],
})
export class PostPictureComponent {

  items: MenuItem[] = [
    {
      label: 'Wybierz obrazek',
      routerLink: 'crop'
    },
    {
      label: 'Wprowadź szczegóły',
      routerLink: 'details'
    },
    {
      label: 'Podsumowanie',
      routerLink: 'overview'
    }
  ];

  constructor(
    private router: Router
  ) {
    if (this.router.url.endsWith('post')) {
      this.router.navigate([this.router.url + '/crop'])
    }
  }

}
