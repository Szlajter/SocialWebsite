import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxSpinnerModule } from "ngx-spinner";
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TimeagoModule } from "ngx-timeago";
import { CollapseModule } from 'ngx-bootstrap/collapse';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    FileUploadModule,
    NgxSpinnerModule.forRoot({
      type: 'ball-clip-rotate'
    }),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    ModalModule.forChild(),
    TimeagoModule.forRoot(),
    CollapseModule.forRoot()  
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    FileUploadModule,
    NgxSpinnerModule,
    BsDatepickerModule,
    PaginationModule,
    ModalModule,
    TimeagoModule,
    CollapseModule
  ]
})
export class SharedModule { }
