import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoggedOutComponent} from "./logged-out.component";

const routes: Routes = [
  {
    path: "",
    component: LoggedOutComponent
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
export class LoggedOutRoutingModule { }
