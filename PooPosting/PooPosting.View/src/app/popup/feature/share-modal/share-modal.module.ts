import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShareModalRoutingModule } from './share-modal-routing.module';
import { ShareModalComponent } from "./share-modal.component";


@NgModule({
  declarations: [
    ShareModalComponent
  ],
  exports: [
    ShareModalComponent
  ],
  imports: [
    CommonModule,
    ShareModalRoutingModule
  ]
})
export class ShareModalModule { }
