import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "logged-out",
    loadChildren: () =>
      import("../logged-out/logged-out.module")
        .then(m => m.LoggedOutModule),
  },
  {
    path: "login",
    loadChildren: () =>
      import("../login/login.module")
        .then(m => m.LoginModule),
  },
  {
    path: "register",
    loadChildren: () =>
      import("../register/register.module")
        .then(m => m.RegisterModule),
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthShellRoutingModule { }
