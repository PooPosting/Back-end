import {NgModule} from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Error404Component } from "./404/error404/error404.component";
import { Error500Component } from "./500/error500/error500.component";
import { Error0Component } from "./0/error0/error0.component";


const routes: Routes = [
  {path: '', redirectTo: "/home", pathMatch: 'full'},

  {
    path: "home",
    loadChildren: () => import('./home/feature/home-shell/home-shell.module')
      .then(m => m.HomeShellModule)
  },
  {
    path: "search",
    loadChildren: () => import('./search/feature/search-shell/search-shell.module')
      .then(m => m.SearchShellModule)
  },
  {
    path: "popular",
    loadChildren: () => import('./popular/feature/popular-shell/popular-shell.module')
      .then(m => m.PopularShellModule)
  },
  {
    path: "tos",
    loadChildren: () => import('./terms-of-service/feature/tos-shell/tos-shell.module')
      .then(m => m.TosShellModule)
  },
  {
    path: "auth",
    loadChildren: () => import('./auth/feature/auth-shell/auth-shell.module')
      .then(m => m.AuthShellModule)
  },
  {
    path: "picture",
    loadChildren: () => import('./picture/feature/picture-shell/picture-shell.module')
      .then(m => m.PictureShellModule)
  },
  {
    path: "account",
    loadChildren: () => import('./account/feature/account-shell/account-shell.module')
      .then(m => m.AccountShellModule)
  },

  { path: "500", component: Error500Component },
  { path: "404", component: Error404Component },
  { path: "0", component: Error0Component },

  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
