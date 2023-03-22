import { Injectable } from '@angular/core';
import {PictureModel} from "../../Models/ApiModels/Get/PictureModel";
import {AccountModel} from "../../Models/ApiModels/Get/AccountModel";
import {UserInfoModel} from "../../Models/UserInfoModel";
import {Subject} from "rxjs";
import {Router} from "@angular/router";
import {HttpServiceService} from "../http/http-service.service";

@Injectable({
  providedIn: 'root'
})
export class CacheServiceService {

  constructor(
    private router: Router,
    private httpService: HttpServiceService
  ) {
    this.loggedOnSubject.next(false);
  }

  mostPopularSite: number = 1;
  mostLikedSite: number = 1;
  newestSite: number = 1;
  randomSite: number = 1;

  loggedOnSubject: Subject<boolean> = new Subject<boolean>();
  cachedPictures: PictureModel[] = [];
  cachedUserAccount?: AccountModel;
  cachedUserInfo?: UserInfoModel;

  public cachePictures(pictures: PictureModel[]): void {
    pictures.forEach((p: PictureModel) => {
      if (!this.cachedPictures.some((r: PictureModel) => r.id == p.id)) {
        this.cachedPictures.push(p);
      }
      while (this.cachedPictures.length > 4) {
        this.cachedPictures.shift();
      }
    });
  }
  public purgeCachePictures(): void {
    this.cachedPictures = [];
  }
  public cacheUserAccount(account: AccountModel): void {
    this.cachedUserAccount = account;
  }
  public cacheUserInfo(userInfo: UserInfoModel): void {
    this.cachedUserInfo = userInfo;
    CacheServiceService.saveLsJwtToken(userInfo.authToken);
    CacheServiceService.saveLsJwtUid(userInfo.uid);
    this.loggedOnSubject.next(true);
  }

  public async updateUserAccount(): Promise<boolean> {
    if (!this.getUserInfo() && !this.getUserInfo()?.uid) return false;
    this.httpService.getAccountRequest(this.getUserInfo()!.uid)
      .subscribe({
        next: (v: AccountModel) => {
          this.cachedUserAccount = v;
          return true;
        }
      })
    return false;
  }

  public isUserAccountCached(): boolean {
    return this.cachedUserAccount !== undefined;
  }
  public arePicturesCached(): boolean {
    return this.cachedPictures.length !== 0;
  }

  public getCachedPictures(): PictureModel[] {
    return this.cachedPictures!;
  }
  public getCachedUserAccount(): AccountModel {
    return this.cachedUserAccount!;
  }

  public logout() {
    this.router.navigate(['auth/logged-out'])
      .then(() => {
        this.loggedOnSubject.next(false);
        this.cachedUserAccount = undefined;
        this.cachedUserInfo = undefined;
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('uid');
      });
  }

  getUserInfo(): UserInfoModel | undefined {
    return this.cachedUserInfo;
  }
  getUserLoggedOnState(): boolean {
    return this.getUserInfo()?.uid != null && this.getUserInfo()?.authToken != null;
  }
  cookiesAlertAccepted() {
    localStorage.setItem('cookiesAlertShown', 'true');
  }
  isCookiesAlertAccepted(): boolean {
    return localStorage.getItem('cookiesAlertShown') ? localStorage.getItem('cookiesAlertShown') === "true": false;
  }

  public getLsJwtToken() {
    return localStorage.getItem('jwtToken');
  }
  public getLsJwtUid() {
    return localStorage.getItem('uid');
  }

  private static saveLsJwtToken(val: string) {
    localStorage.setItem('jwtToken', val);
  }
  private static saveLsJwtUid(val: string) {
    localStorage.setItem('uid', val);
  }

}
