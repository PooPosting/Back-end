import { Injectable } from '@angular/core';
import {CanActivate} from '@angular/router';
import {LocationServiceService} from "../../helpers/location-service.service";
import {AppCacheService} from "../../state/app-cache.service";

@Injectable({
  providedIn: 'root'
})
export class IsLoggedOnRouteGuardGuard implements CanActivate {
  constructor(
    private cacheService: AppCacheService,
    private locationService: LocationServiceService,
  ) {
  }

  canActivate(): boolean {
    if (!this.cacheService.getUserLoggedOnState()) {
      return true;
    } else {
      this.locationService.goBack();
      return false;
    }
  }

}
