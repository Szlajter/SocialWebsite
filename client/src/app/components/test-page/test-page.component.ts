import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-test-page',
  templateUrl: './test-page.component.html',
  styleUrls: ['./test-page.component.css']
})
export class TestPageComponent {
  baseUrl = environment.apiUrl;
  validationErrors: string[] = []; 

  constructor(private http: HttpClient) {};

  get404error() {
    this.http.get(this.baseUrl + 'ExpectionTes/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400error() {
    this.http.get(this.baseUrl + 'ExceptionTest/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500error() {
    this.http.get(this.baseUrl + 'ExceptionTest/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401error() {
    this.http.get(this.baseUrl + 'ExceptionTest/auth').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400Validationerror() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => {
        console.log(error);
        this.validationErrors = error;
      }
    })
  }
}
