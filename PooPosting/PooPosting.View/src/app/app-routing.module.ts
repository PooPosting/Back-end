import {NgModule} from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Error404Component } from "./Modules/Shared/pages/error404/error404.component";
import { Error500Component } from "./Modules/Shared/pages/error500/error500.component";
import { Error0Component } from "./Modules/Shared/pages/error0/error0.component";
import { HomepageComponent } from "./Modules/Home/pages/homepage/homepage.component";
import { PopularComponent } from "./Modules/Home/pages/popular/popular.component";
import {LogsComponent} from "./Modules/Report/pages/logs/logs.component";
import {SearchComponent} from "./Modules/Home/pages/search/search.component";


const routes: Routes = [
  {path: '', redirectTo: "/home", pathMatch: 'full'},

  {path: "home", component: HomepageComponent},
  {path: "search", component: SearchComponent},
  {path: "report", component: LogsComponent},

  {path: "popular", component: PopularComponent},

  // lazy loaded modules
  {
    path: "tos",
    loadChildren: () => import('./Modules/Tos/tos.module').then(m => m.TosModule)
  },
  {
    path: "auth",
    loadChildren: () => import('./Modules/Auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: "picture",
    loadChildren: () => import('./Modules/Picture/picture.module').then(m => m.PictureModule)
  },
  {
    path: "account",
    loadChildren: () => import('./Modules/Account/account.module').then(m => m.AccountModule)
  },



  { path: "error500", component: Error500Component },
  { path: "error404", component: Error404Component },
  { path: "error0", component: Error0Component },

  { path: '**', redirectTo: '/error404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
