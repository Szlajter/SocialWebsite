import { Component } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  model: any = {}
  
  constructor(public accountService: AccountService, private router: Router) {}

  register(){
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response);
        this.router.navigateByUrl('');
      },
      error: error => console.log(error)
    })
  }
}
