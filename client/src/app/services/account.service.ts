import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../models/user';
import { environment } from 'src/environments/environment.development';
import { StatusService } from './status.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private statusService: StatusService, private router: Router) { }
  
  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((user) => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    this.statusService.createHubConnection(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.statusService.stopHubConnection();
    window.location.reload();
  }

  //atob is marked as deprecated, but it has a second overload that remains functional.
  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]))
  }

  isTokenExpired(token: string): boolean {
    try {
      const decodedToken = this.getDecodedToken(token);
      const date = new Date(0);
      date.setUTCSeconds(decodedToken.exp);
      return date.valueOf() < new Date().valueOf();
    }
    catch (error) {
      return false;
    }
  }
}
