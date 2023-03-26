import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "picture",
    loadChildren: () => import("../picture-details-modal/picture-details-modal.module")
      .then(m => m.PictureDetailsModalModule)
  },
  {
    path: "share",
    loadChildren: () => import("../share-modal/share-modal.module")
      .then(m => m.ShareModalModule)
  },
  // {
  //   path: "share-picture",
  // },
  // {
  //   path: "share-account",
  // },
  {
    path: "**",
    redirectTo: "/404"
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PopupShellRoutingModule { }
