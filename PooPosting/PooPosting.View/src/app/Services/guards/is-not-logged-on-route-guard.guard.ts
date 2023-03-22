import { Injectable } from '@angular/core';
import {CanActivate, Router} from '@angular/router';
import {MessageService} from "primeng/api";
import {CacheServiceService} from "../data/cache-service.service";

@Injectable({
  providedIn: 'root'
})
export class IsNotLoggedOnRouteGuardGuard implements CanActivate {
  constructor(
    private cacheService: CacheServiceService,
    private messageService: MessageService,
    private router: Router
  ) { }

  canActivate(): boolean {
    if (this.cacheService.getUserLoggedOnState()) {
      return true;
    } else {
      this.router.navigate(['/home']);
      return false;
    }
  }
}
