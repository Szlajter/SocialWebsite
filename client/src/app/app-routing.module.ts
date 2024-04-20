import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomePageComponent } from './components/home-page/home-page.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { TestPageComponent } from './components/test-page/test-page.component';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { ServerErrorPageComponent } from './components/server-error-page/server-error-page.component';
import { MembersPageComponent } from './components/members-page/members-page.component';
import { ProfilePageComponent } from './components/profile-page/profile-page.component';
import { EditProfilePageComponent } from './components/edit-profile-page/edit-profile-page.component';
import { authGuard } from './guards/auth.guard';
import { loggedInGuard } from './guards/logged-in-auth.guard'
import { PendingChangesGuard } from './guards/pending-changes.guard';
import { MessagesComponent } from './components/messages/messages.component';
import { AdminPanelComponent } from './components/admin/admin-panel/admin-panel.component';
import { adminGuard } from './guards/admin.guard';
import { CommentPageComponent } from './components/comment-page/comment-page.component';

const routes: Routes = [
  {path: '', component: HomePageComponent},
  {path: '', 
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'edit-profile-page', component: EditProfilePageComponent, canDeactivate: [PendingChangesGuard]},
      {path: 'messages', component: MessagesComponent},
      {path: 'post/:id', component: CommentPageComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard]}
    ]
  },
  {path: '', 
    runGuardsAndResolvers: 'always',
    canActivate: [loggedInGuard],
    children: [
      {path: 'login-page', component: LoginPageComponent},
      {path: 'register-page', component: RegisterPageComponent},
    ]
  },
  {path: 'members-page', component: MembersPageComponent},
  {path: 'profile-page/:username', component: ProfilePageComponent},
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
