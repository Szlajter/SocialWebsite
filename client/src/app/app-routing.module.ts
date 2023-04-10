import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { TestPageComponent } from './components/test-page/test-page.component';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { ServerErrorPageComponent } from './components/server-error-page/server-error-page.component';

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: 'login-page', component: LoginPageComponent},
  {path: 'register-page', component: RegisterPageComponent},
  {path: 'test-page', component: TestPageComponent},
  {path: 'not-found-page', component: NotFoundPageComponent},
  {path: 'server-error-page', component: ServerErrorPageComponent},
  {path: '**', component: NotFoundPageComponent, pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
