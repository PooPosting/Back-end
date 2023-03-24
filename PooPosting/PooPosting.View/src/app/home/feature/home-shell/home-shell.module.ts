import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeShellRoutingModule } from './home-shell-routing.module';
import {HomepageModule} from "../homepage/homepage.module";


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    HomepageModule,
    HomeShellRoutingModule
  ]
})
export class HomeShellModule { }
