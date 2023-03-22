import { Injectable } from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import { Observable } from 'rxjs';
import {CacheServiceService} from "../data/cache-service.service";

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {
  constructor(
    private cacheService: CacheServiceService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if (this.cacheService.getUserLoggedOnState()) {
      let tokenizedReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.cacheService.getUserInfo()!.authToken}`
        }
      });
      return next.handle(tokenizedReq);
    }

    return next.handle(req);

  }
}
