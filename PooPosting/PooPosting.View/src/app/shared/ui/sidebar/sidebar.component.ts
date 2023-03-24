import { Component, OnInit } from '@angular/core';
import { UserState } from 'src/app/shared/utils/models/userState';
import {Router} from "@angular/router";
import {MenuLinksService} from "../../state/menu-links.service";
import {SidebarItem} from "../../utils/models/sidebarItem";
import {AppCacheService} from "../../state/app-cache.service";
import {SidebarLink} from "../../utils/models/sidebarLink";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  userInfo: UserState | undefined;
  sidebarItems: SidebarItem[];
  sidebarLinks: SidebarLink[];
  loggedIn!: boolean;

  constructor(
    private cacheService: AppCacheService,
    private menuService: MenuLinksService,
    private router: Router,
  ) {
    this.sidebarItems = menuService.getSidebarItems();
    this.sidebarLinks = this.menuService.getSidebarLinks();
    this.loggedIn = this.cacheService.getUserLoggedOnState();
  }

  ngOnInit(): void {
    this.userInfo = this.cacheService.getUserInfo();
    this.cacheService.loggedOnSubject.subscribe({
      next: (val: boolean) => {
        this.loggedIn = val;
        this.userInfo = this.cacheService.getUserInfo();
      }
    })
  }

  logout() {
    this.cacheService.logout();
  }



  toPostPicture() {
    this.router.navigate(['picture/post']);
  }

  toMyAccount() {
    this.router.navigate(['my-account']);
  }

  toMyPictures() {
    this.router.navigate(['my-account/picture']);
  }

}
