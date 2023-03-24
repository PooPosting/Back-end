import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PostOverviewRoutingModule } from './post-overview-routing.module';
import {PostOverviewComponent} from "./post-overview.component";
import {CardModule} from "primeng/card";
import {TagModule} from "primeng/tag";
import {ButtonModule} from "primeng/button";


@NgModule({
  declarations: [
    PostOverviewComponent
  ],
  exports: [
    PostOverviewComponent
  ],
  imports: [
    CommonModule,
    PostOverviewRoutingModule,
    CardModule,
    TagModule,
    ButtonModule
  ]
})
export class PostOverviewModule { }
