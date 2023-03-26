import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomepageComponent} from "./homepage.component";

const routes: Routes = [
  {
    path: "",
    component: HomepageComponent,
    children: [
      {
        path: "popup",
        loadChildren: () => import('../../../popup/feature/popup-shell/popup-shell-routing.module')
          .then(m => m.PopupShellRoutingModule)
      },
    ]
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
export class HomepageRoutingModule { }
