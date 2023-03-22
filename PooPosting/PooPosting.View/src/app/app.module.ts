import { NgModule} from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UIModule } from "./Modules/UI/ui.module";
import { TokenInterceptorService } from './Services/interceptors/token-interceptor.service';
import { HttpErrorInterceptorService } from './Services/interceptors/http-error-interceptor.service';
import { MessageService} from "primeng/api";
import { PrimeNgModule } from "./Modules/Prime-ng/prime-ng.module";
import { environment } from '../environments/environment';
import { SharedModule } from "./Modules/Shared/shared.module";
import { HomeModule } from "./Modules/Home/home.module";
import { DateAgoPipe } from './Pipes/date-ago.pipe';
import { ReportModule } from "./Modules/Report/report.module";
import { BrowserModule } from "@angular/platform-browser";
import { ServiceWorkerModule } from "@angular/service-worker";
import { TitleCasePipe } from "@angular/common";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    AppRoutingModule,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', {enabled: environment.production}),
    PrimeNgModule,
    SharedModule,
    ReportModule,
    HomeModule,
    UIModule,
  ],
  providers: [
    TitleCasePipe,
    MessageService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptorService,
      multi: true,
    },
  ],
  exports: [
    DateAgoPipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
