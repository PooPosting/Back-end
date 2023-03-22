import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from "./pages/login/login.component";
import {IsLoggedOnRouteGuardGuard} from "../../Services/guards/is-logged-on-route-guard.guard";
import {RegisterComponent} from "./pages/register/register.component";
import {LoggedOutComponent} from "./pages/logged-out/logged-out.component";

const routes: Routes = [
  {
    path: "login",
    component: LoginComponent,
    canActivate: [IsLoggedOnRouteGuardGuard]
  },
  {
    path: "register",
    component: RegisterComponent,
    canActivate: [IsLoggedOnRouteGuardGuard]
  },
  {
    path: "logged-out",
    component: LoggedOutComponent
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
export class AuthRoutingModule { }
