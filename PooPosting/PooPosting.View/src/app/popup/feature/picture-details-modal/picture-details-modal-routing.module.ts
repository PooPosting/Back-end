import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PictureDetailsModalComponent} from "./picture-details-modal.component";

const routes: Routes = [
  {
    path: ":id",
    component: PictureDetailsModalComponent
  },
  {
    path: "**",
    redirectTo: "/404"
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PictureDetailsModalRoutingModule { }
