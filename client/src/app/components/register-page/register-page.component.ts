import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {
  model: any = {}
  registerForm: FormGroup = new FormGroup({});
  
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(15)]),
      confirmPassword: new FormControl('', [Validators.required, this.checkPasswords('password')])
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
  
  checkPasswords(matchTo: string): ValidatorFn { 
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notSame: true}
    }
  }



  register(){
    console.log(this.registerForm.value);
    // this.accountService.register(this.model).subscribe({
    //   next: _ => this.router.navigateByUrl(''),
    //   error: error => this.toastr.error(error.error)
    // })
  }
}
