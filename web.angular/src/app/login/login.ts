import { CommonModule } from '@angular/common';
import { Component, model, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Components } from '../../components';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ...Components],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  constructor(private router: Router) { }

  model = model({
    username: '',
    password: '',
    rememberMe: false,
  });
  // Local state as signals
  firstName = signal('');
  value = model<string>('');

  onSubmit() {
    this.router.navigateByUrl('/table-page');
    // alert(`username = ${this.model().username}, password = ${this.model().password}, rememberMe = ${this.model().rememberMe}`);
  }

  onFirstNameChange(event: string) {
    // alert(`First name changed to: ${JSON.stringify(event)}`);
  }

  reset() {
    this.model.set({
      username: '',
      password: '',
      rememberMe: false,
    });
    this.firstName.set('');
  }

}
