import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PictureDetailsComponent} from "./pages/picture-details/picture-details.component";
import {SharedModule} from "../Shared/shared.module";
import {PrimeNgModule} from "../Prime-ng/prime-ng.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {PictureRoutingModule} from "./picture-routing.module";
import {PostPictureComponent} from "./pages/post-picture/post-picture.component";
import { PostCropComponent } from './pages/post-picture/post-crop/post-crop.component';
import { PostDetailsComponent } from './pages/post-picture/post-details/post-details.component';
import { PostOverviewComponent } from './pages/post-picture/post-overview/post-overview.component';

@NgModule({
    declarations: [
      PictureDetailsComponent,
      PostPictureComponent,
      PostCropComponent,
      PostDetailsComponent,
      PostOverviewComponent,
    ],
    imports: [
      FormsModule,
      CommonModule,
      SharedModule,
      PrimeNgModule,
      ReactiveFormsModule,
      PictureRoutingModule,
    ],
    exports: [
      PictureDetailsComponent,
      PostPictureComponent,
    ]
})
export class PictureModule { }
