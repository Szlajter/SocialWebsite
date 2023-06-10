import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxSpinnerModule } from "ngx-spinner";
import { FileUploadModule } from 'ng2-file-upload';

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
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    FileUploadModule,
    NgxSpinnerModule
  ]
})
export class SharedModule { }
