import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {Location} from "@angular/common";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";

@Component({
  selector: 'app-my-account',
  template: '',
  styles: ['']
})
export class MyAccountComponent implements OnInit {
  constructor(
    private cacheService: CacheServiceService,
    private location: Location,
    private router: Router,
  ) { }

  ngOnInit(): void {
    if(this.cacheService.getUserLoggedOnState()) {
      this.router.navigate([`/account/${this.cacheService.getUserInfo()!.uid}`]);
      this.location.go('/home');
    } else {
      this.router.navigate(['/home']);
    }
  }

}
