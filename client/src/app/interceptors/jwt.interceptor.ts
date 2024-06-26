import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService, private toastr: ToastrService) {}
  //adds jwt token to logged user requests
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if(user && user.token){
          if (this.accountService.isTokenExpired(user.token)) {
            this.accountService.logout();
            this.toastr.error("Token expired. User logged out");
          }
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${user.token}`
            }
          })
        }
      }
    })

    return next.handle(request);
  }
}
