import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SearchRoutingModule } from './search-routing.module';
import {SearchComponent} from "./search.component";
import {SearchPanelModule} from "../../../shared/ui/search-panel/search-panel.module";
import {GlobalPaginatorModule} from "../../../shared/ui/global-paginator/global-paginator.module";
import {PicturePreviewModule} from "../../../shared/ui/picture-preview/picture-preview.module";
import {AccountPreviewModule} from "../../../shared/ui/account-preview/account-preview.module";


@NgModule({
  declarations: [
    SearchComponent
  ],
  imports: [
    CommonModule,
    SearchRoutingModule,
    SearchPanelModule,
    GlobalPaginatorModule,
    PicturePreviewModule,
    AccountPreviewModule
  ]
})
export class SearchModule { }
