import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RulebookComponent} from "./feature/rulebook/rulebook.component";

const routes: Routes = [
  {
    path: "rulebook",
    component: RulebookComponent
  },
  {
    path: "**",
    redirectTo: "/error404"
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TosRoutingModule { }
