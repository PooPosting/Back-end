import { Injectable } from '@angular/core';
import {MenuItem} from "../utils/models/menuItem";
import {SidebarItem} from "../utils/models/sidebarItem";
import {SidebarLink} from "../utils/models/sidebarLink";

@Injectable({
  providedIn: 'root'
})
export class MenuLinksService {
  sidebarItems!: SidebarItem[];
  menuItems!: MenuItem[];
  sidebarLinks!: SidebarLink[];

  constructor(
  ) {
    this.refreshItems();
  }

  getMenuItems(): MenuItem[] {
    return this.menuItems;
  }
  getSidebarItems(): SidebarItem[] {
    return this.sidebarItems;
  }
  getSidebarLinks(): SidebarLink[] {
    return this.sidebarLinks;
  }

  refreshItems() {
    this.menuItems = [
      {
        label: "Strona główna",
        class: "bi bi-house-door-fill",
        url: "home"
      },
      {
        label: "Popularne",
        class: "bi bi-graph-up-arrow",
        url: "popular"
      },
      {
        label: "Odkrywaj",
        class: "bi bi-search",
        url: "search"
      },
    ];
    this.sidebarItems = [
      {
        label: "Zaloguj się lub zarejestruj!",
        class: "bi bi-balloon-heart",
        showWhileLoggedIn: false,
        showWhileLoggedOut: true,
        menuItems: [
          {
            label: "Zaloguj się",
            class: "bi bi-box-arrow-in-left",
            url: "auth/login"
          },
          {
            label: "Zarejestruj się",
            class: "bi bi-person-plus",
            url: "auth/register"
          }
        ]
      },
      {
        label: "Moje Obrazki",
        class: "bi bi-images",
        showWhileLoggedIn: true,
        showWhileLoggedOut: false,
        menuItems: [
          {
            label: "Wstaw obrazek",
            class: "bi bi-cloud-upload",
            url: "picture/post"
          }
        ]
      },
      {
        label: "Moje Konto",
        class: "bi bi-person",
        showWhileLoggedIn: true,
        showWhileLoggedOut: false,
        menuItems: [
          {
            label: "Konto",
            class: "bi bi-person-circle",
            url: "account/my-account",
          },
          {
            label: "Wyloguj",
            class: "bi bi-box-arrow-right",
            url: "auth/logged-out"
          }
        ]
      },
    ];
    this.sidebarLinks = [
      {
        label: "Polityka prywatności",
        url: "https://migra.ml/polityka-prywatnosci"
      },
      {
        label: "Regulamin witryny",
        url: "https://pictures.migra.ml/tos/rulebook"
      },
      {
        label: "O Twórcy",
        url: "https://migra.ml"
      }
    ]
  }
}
