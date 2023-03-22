import { Injectable } from '@angular/core';
import {CanActivate} from '@angular/router';
import {LocationServiceService} from "../helpers/location-service.service";
import {CacheServiceService} from "../data/cache-service.service";

@Injectable({
  providedIn: 'root'
})
export class IsLoggedOnRouteGuardGuard implements CanActivate {
  constructor(
    private cacheService: CacheServiceService,
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
