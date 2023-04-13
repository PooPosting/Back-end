import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostCropComponent } from './post-crop/post-crop.component';
import { PostDetailsComponent } from './post-details/post-details.component';
import { PostOverviewComponent } from './post-overview/post-overview.component';
import {PostPictureComponent} from "./post-picture.component";

const routes: Routes = [
  {
    path: "",
    component: PostPictureComponent,
    children: [
      {
        path: "crop",
        component: PostCropComponent
      },
      {
        path: "details",
        component: PostDetailsComponent
      },
      {
        path: "overview",
        component: PostOverviewComponent
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
