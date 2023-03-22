import { NgModule } from '@angular/core';
import {ChipsModule} from "primeng/chips";
import {InputTextModule} from "primeng/inputtext";
import {ButtonModule} from "primeng/button";
import {RippleModule} from "primeng/ripple";
import {PasswordModule} from "primeng/password";
import {InputMaskModule} from "primeng/inputmask";
import {FileUploadModule} from "primeng/fileupload";
import {InputTextareaModule} from "primeng/inputtextarea";
import {CardModule} from "primeng/card";
import {AccordionModule} from "primeng/accordion";
import {PaginatorModule} from "primeng/paginator";
import {ToastModule} from "primeng/toast";
import {MessageModule} from "primeng/message";
import {DataViewModule} from "primeng/dataview";
import {DividerModule} from "primeng/divider";
import {TagModule} from "primeng/tag";
import {DialogModule} from "primeng/dialog";
import {CaptchaModule} from "primeng/captcha";
import {KeyFilterModule} from "primeng/keyfilter";
import {SelectButtonModule} from "primeng/selectbutton";
import {ScrollPanelModule} from "primeng/scrollpanel";
import {MenubarModule} from "primeng/menubar";
import {SidebarModule} from "primeng/sidebar";
import {ProgressSpinnerModule} from "primeng/progressspinner";
import {CarouselModule} from "primeng/carousel";
import {TableModule} from "primeng/table";
import {SkeletonModule} from "primeng/skeleton";
import {ListboxModule} from "primeng/listbox";
import {OverlayPanelModule} from "primeng/overlaypanel";
import {StepsModule} from "primeng/steps";

const PrimeNgComponents = [
  ChipsModule,
  RippleModule,
  InputTextModule,
  ButtonModule,
  PasswordModule,
  InputMaskModule,
  FileUploadModule,
  InputTextareaModule,
  CardModule,
  AccordionModule,
  PaginatorModule,
  ToastModule,
  MessageModule,
  DividerModule,
  TagModule,
  DialogModule,
  CaptchaModule,
  KeyFilterModule,
  SelectButtonModule,
  DataViewModule,
  ScrollPanelModule,
  MenubarModule,
  SidebarModule,
  ProgressSpinnerModule,
  CarouselModule,
  TableModule,
  SkeletonModule,
  ListboxModule,
  OverlayPanelModule,
  StepsModule,

]

@NgModule({
  declarations: [],
  imports: [
    PrimeNgComponents
  ],
  exports: [
    PrimeNgComponents
  ]
})
export class PrimeNgModule { }
