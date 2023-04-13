import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {MessageService} from "primeng/api";
import {AppCacheService} from "../../state/app-cache.service";

@Injectable({
  providedIn: 'root'
})
export class IsNotLoggedOnRouteGuardGuard  {
  constructor(
    private cacheService: AppCacheService,
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
