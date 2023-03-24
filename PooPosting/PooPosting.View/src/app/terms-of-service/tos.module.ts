import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RulebookComponent } from './feature/rulebook/rulebook.component';
import {TosRoutingModule} from "./tos-routing.module";

@NgModule({
  declarations: [
    RulebookComponent
  ],
  imports: [
    CommonModule,
    TosRoutingModule,
  ]
})
export class TosModule { }
