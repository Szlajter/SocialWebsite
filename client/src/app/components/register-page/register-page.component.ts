import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  minDate: Date = new Date();
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;
  
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService, private fb: FormBuilder)  {}

  ngOnInit(): void {
    this.initializeForm();

    this.minDate.setFullYear(this.minDate.getFullYear() - 120);
    this.maxDate.setFullYear(this.maxDate.getFullYear());
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
      gender: ['male'],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(15)]],
      confirmPassword: ['', [Validators.required, this.checkPasswords('password')]]
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
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value)
    const values = {...this.registerForm.value, dateOfBirth: dob};
    
    this.accountService.register(values).subscribe({
      next: _ => this.router.navigateByUrl('/members-page'),
       error: error => this.validationErrors = error
    })
  }

  getDateOnly(dob: Date | undefined) {
    if (!dob) return;

    let date = new Date(dob);
    return new Date(date.setMinutes(date.getMinutes() - date.getTimezoneOffset())).toISOString().slice(0,10);
  }
}
