import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PopularComponent} from "./popular.component";

const routes: Routes = [
  {
    path: "",
    component: PopularComponent
  },
  {
    path: "**",
    redirectTo: ""
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PopularpageRoutingModule { }
