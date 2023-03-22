import {Component, Input, OnInit} from '@angular/core';
import {MenuItem} from "../../../Models/MenuModels/MenuItem";
import {MenusServiceService} from "../../../Services/data/menus-service.service";
import {Router} from "@angular/router";
import {LocationServiceService} from "../../../Services/helpers/location-service.service";
import {HttpParamsServiceService} from "../../../Services/http/http-params-service.service";
import {CacheServiceService} from "../../../Services/data/cache-service.service";
import {SidebarItem} from "../../../Models/MenuModels/SidebarItem";


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
    private cacheService: CacheServiceService,
    private locationService: LocationServiceService,
    private menuService: MenusServiceService,
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
