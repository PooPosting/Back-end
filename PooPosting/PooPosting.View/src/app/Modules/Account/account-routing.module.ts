import {MyAccountComponent} from "./pages/my-account/my-account.component";
import {IsNotLoggedOnRouteGuardGuard} from "../../Services/guards/is-not-logged-on-route-guard.guard";
import {AccountDetailsComponent} from "./pages/account-details/account-details.component";
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: "my-account",
    component: MyAccountComponent,
    canActivate: [IsNotLoggedOnRouteGuardGuard],
  },
  {
    path: ":id",
    component: AccountDetailsComponent,
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
export class AccountRoutingModule { }
