import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PostOverviewComponent} from "./post-overview.component";

const routes: Routes = [
  {
    path: "",
    component: PostOverviewComponent
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
export class PostOverviewRoutingModule { }
