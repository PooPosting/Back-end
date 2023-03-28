import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PictureDetailsComponent} from "./picture-details.component";

const routes: Routes = [
  {
    path: "",
    component: PictureDetailsComponent
  },
  {
    path: "popup",
    loadChildren: () => import('../../../popup/feature/popup-shell/popup-shell.module')
      .then(m => m.PopupShellModule)
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
export class PictureDetailsRoutingModule { }
