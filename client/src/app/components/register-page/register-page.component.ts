import { Component } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  model: any = {}
  
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {}

  register(){
    this.accountService.register(this.model).subscribe({
      next: _ => this.router.navigateByUrl(''),
      error: error => this.toastr.error(error.error)
    })
  }
}
