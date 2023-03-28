import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VoteButtonComponent } from './vote-button.component';
import {ButtonModule} from "primeng/button";



@NgModule({
  declarations: [
    VoteButtonComponent
  ],
  exports: [
    VoteButtonComponent
  ],
  imports: [
    CommonModule,
    ButtonModule
  ]
})
export class VoteButtonModule { }
