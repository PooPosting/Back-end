import {Component, Input, OnInit} from '@angular/core';
import {MenuItem} from "../../utils/models/menuItem";
import {MenuLinksService} from "../../state/menu-links.service";
import {Router} from "@angular/router";
import {LocationServiceService} from "../../helpers/location-service.service";
import {HttpParamsServiceService} from "../../data-access/http-params-service.service";
import {AppCacheService} from "../../state/app-cache.service";
import {SidebarItem} from "../../utils/models/sidebarItem";


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit{
  @Input() appTitle!: string;
  currentHomePage!: number;
  menuItems: MenuItem[];
  menuExpandableItems: SidebarItem[];
  showSidebar: boolean = false;
  loggedIn: boolean;

  constructor(
    private cacheService: AppCacheService,
    private locationService: LocationServiceService,
    private menuService: MenuLinksService,
    private router: Router,
    private paramsService: HttpParamsServiceService,
  ) {
    this.menuItems = menuService.getMenuItems();
    this.menuExpandableItems = menuService.getSidebarItems();
    this.loggedIn = this.cacheService.getUserLoggedOnState();
  }

  ngOnInit(): void {
    this.cacheService.loggedOnSubject.subscribe({
      next: (val: boolean) => {
        this.loggedIn = val;
      }
    })
    this.currentHomePage = this.paramsService.getPageNumber();
  }

  logout() {
    this.cacheService.logout();
    this.showSidebar = false;
  }

}
