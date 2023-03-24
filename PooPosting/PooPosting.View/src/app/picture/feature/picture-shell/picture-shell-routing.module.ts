import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {IsNotLoggedOnRouteGuardGuard} from "../../../shared/utils/guards/is-not-logged-on-route-guard.guard";

const routes: Routes = [
  {
    path: "post",
    loadChildren: () => import('../post-picture/post-picture.module')
      .then(m => m.PostPictureModule),
    canActivate: [IsNotLoggedOnRouteGuardGuard],
  },
  {
    path: ":id",
    loadChildren: () => import('../picture-details/picture-details.module')
      .then(m => m.PictureDetailsModule)
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
export class PictureShellRoutingModule { }
