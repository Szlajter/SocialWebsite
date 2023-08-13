import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NavComponent } from './components/nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { SharedModule } from './modules/shared.module';
import { TestPageComponent } from './components/test-page/test-page.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { ServerErrorPageComponent } from './components/server-error-page/server-error-page.component';
import { MembersPageComponent } from './components/members-page/members-page.component';
import { ProfilePageComponent } from './components/profile-page/profile-page.component';
import { MemberCardComponent } from './components/member-card/member-card.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { EditProfilePageComponent } from './components/edit-profile-page/edit-profile-page.component';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { AvatarUploaderComponent } from './components/avatar-uploader/avatar-uploader.component';
import { TextInputComponent } from './components/forms/text-input/text-input.component';
import { DatepickerComponent } from './components/forms/datepicker/datepicker.component';
import { INTL_LOCALES } from 'angular-ecmascript-intl';
import { MessagesComponent } from './components/messages/messages.component';
import { ConversationComponent } from './components/conversation/conversation.component';



@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomePageComponent,
    LoginPageComponent,
    RegisterPageComponent,
    TestPageComponent,
    NotFoundPageComponent,
    ServerErrorPageComponent,
    MembersPageComponent,
    ProfilePageComponent,
    MemberCardComponent,
    EditProfilePageComponent,
    AvatarUploaderComponent,
    TextInputComponent,
    DatepickerComponent,
    MessagesComponent,
    ConversationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    //sets timeago pipe language
    {provide: INTL_LOCALES, useValue: "en-US",}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
