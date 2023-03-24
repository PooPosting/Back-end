import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PostDetailsComponent} from "./post-details.component";

const routes: Routes = [
  {
    path: "",
    component: PostDetailsComponent
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
export class PostDetailsRoutingModule { }
