import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "rulebook",
    loadChildren: () => import('../rulebook/rulebook-routing.module')
      .then(m => m.RulebookRoutingModule)
  },
  {
    path: "privacy-policy",
    loadChildren: () => import('../privacy-policy/privacy-policy.module')
      .then(m => m.PrivacyPolicyModule)
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
export class TosShellRoutingModule { }
