import { Component, OnInit } from '@angular/core';
import { UserInfoModel } from 'src/app/Models/UserInfoModel';
import {Router} from "@angular/router";
import {MenusServiceService} from "../../../Services/data/menus-service.service";
import {SidebarItem} from "../../../Models/MenuModels/SidebarItem";
import {CacheServiceService} from "../../../Services/data/cache-service.service";
import {SidebarLink} from "../../../Models/MenuModels/SidebarLink";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  userInfo: UserInfoModel | undefined;
  sidebarItems: SidebarItem[];
  sidebarLinks: SidebarLink[];
  loggedIn!: boolean;

  constructor(
    private cacheService: CacheServiceService,
    private menuService: MenusServiceService,
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
