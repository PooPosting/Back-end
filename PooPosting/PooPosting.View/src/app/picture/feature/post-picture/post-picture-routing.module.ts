import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PostPictureComponent} from "./post-picture.component";

const routes: Routes = [
  {
    path: "",
    redirectTo: "crop",
    component: PostPictureComponent,
    children: [
      {
        path: "crop",
        loadChildren: () => import('./post-crop/post-crop.module')
          .then(m => m.PostCropModule)
      },
      {
        path: "details",
        loadChildren: () => import('./post-details/post-details.module')
          .then(m => m.PostDetailsModule)
      },
      {
        path: "overview",
        loadChildren: () => import('./post-overview/post-overview.module')
          .then(m => m.PostOverviewModule)
      },
    ]
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
export class PostPictureRoutingModule { }
