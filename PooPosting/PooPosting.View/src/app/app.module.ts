import { NgModule} from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TokenInterceptorService } from './shared/utils/interceptors/token-interceptor.service';
import { HttpErrorInterceptorService } from './shared/utils/interceptors/http-error-interceptor.service';
import { MessageService} from "primeng/api";
import { environment } from '../environments/environment';
import { BrowserModule } from "@angular/platform-browser";
import { ServiceWorkerModule } from "@angular/service-worker";
import { TitleCasePipe } from "@angular/common";
import {HomeShellModule} from "./home/feature/home-shell/home-shell.module";
import {NavbarModule} from "./shared/ui/navbar/navbar.module";
import {SidebarModule} from "./shared/ui/sidebar/sidebar.module";
import {ToastModule} from "primeng/toast";
import {Error0Component} from "./0/error0/error0.component";
import {Error404Component} from "./404/error404/error404.component";
import {Error500Component} from "./500/error500/error500.component";
import {ButtonModule} from "primeng/button";
import {RippleModule} from "primeng/ripple";

@NgModule({
  declarations: [
    AppComponent,
    Error0Component,
    Error404Component,
    Error500Component
  ],
  imports: [
    AppRoutingModule,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    ServiceWorkerModule.register('ngsw-worker.js', {enabled: environment.production}),
    HomeShellModule,
    NavbarModule,
    SidebarModule,
    ToastModule,
    ButtonModule,
    RippleModule,
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
  exports: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
