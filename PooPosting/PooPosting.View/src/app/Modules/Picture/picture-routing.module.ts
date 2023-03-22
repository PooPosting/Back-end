import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PostPictureComponent} from "./pages/post-picture/post-picture.component";
import {IsNotLoggedOnRouteGuardGuard} from "../../Services/guards/is-not-logged-on-route-guard.guard";
import {PictureDetailsComponent} from "./pages/picture-details/picture-details.component";
import {PostCropComponent} from "./pages/post-picture/post-crop/post-crop.component";
import {PostDetailsComponent} from "./pages/post-picture/post-details/post-details.component";
import {PostOverviewComponent} from "./pages/post-picture/post-overview/post-overview.component";

const routes: Routes = [
  {
    path: "post",
    component: PostPictureComponent,
    canActivate: [IsNotLoggedOnRouteGuardGuard],
    children: [
      {
        path: "crop",
        component: PostCropComponent,
      },
      {
        path: "details",
        component: PostDetailsComponent,
      },
      {
        path: "overview",
        component: PostOverviewComponent,
      },
    ]
  },
  {
    path: ":id",
    component: PictureDetailsComponent,
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
export class PictureRoutingModule { }
