import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PostCropComponent} from "./post-crop.component";

const routes: Routes = [
  {
    path: "",
    component: PostCropComponent
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
export class PostCropRoutingModule { }
