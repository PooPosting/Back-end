import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PrimeNgModule} from "../Prime-ng/prime-ng.module";
import {LogsComponent} from "./pages/logs/logs.component";

@NgModule({
  declarations: [
    LogsComponent
  ],
  imports: [
    CommonModule,
    PrimeNgModule
  ]
})
export class ReportModule { }
