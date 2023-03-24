import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RulebookComponent} from "./rulebook.component";

const routes: Routes = [
  {
    path: "",
    component: RulebookComponent
  }
];

@NgModule({
  declarations: [
    RulebookComponent
  ],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule,
    RulebookComponent
  ]
})
export class RulebookRoutingModule { }
