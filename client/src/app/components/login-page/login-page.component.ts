import { Component } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  model: any = {}
  
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {}

  login(){
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl(''),
    })
  }
  
}
