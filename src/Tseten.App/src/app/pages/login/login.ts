// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService, LocalStorageService, NavigationService, loginCredentialsKey } from '../../@core';

interface LoginCredentials {
  username: string;
  password: string;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login implements OnInit {
  private _authService = inject(AuthService);
  private _localStorageService = inject(LocalStorageService);
  private _navigationService = inject(NavigationService);

  username = '';
  password = '';
  rememberMe = false;
  error = '';

  loginForm: FormGroup = new FormGroup({
    username: new FormControl(this.username, [Validators.required]),
    password: new FormControl(this.password, [Validators.required]),
    rememberMe: new FormControl(this.rememberMe)
  });

  ngOnInit(): void {
    const loginCredentials = this._localStorageService.get({ key: loginCredentialsKey }) as LoginCredentials;

    if (loginCredentials) {
      this.username = loginCredentials.username;
      this.password = loginCredentials.password;
      this.rememberMe = true;
      this.loginForm.patchValue({
        username: this.username,
        password: this.password,
        rememberMe: true
      });
    }
  }

  handleLogin(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const credentials = this.loginForm.value;
    this.error = '';

    if (credentials.rememberMe) {
      this._localStorageService.put({
        key: loginCredentialsKey,
        value: { username: credentials.username, password: credentials.password }
      });
    } else {
      this._localStorageService.put({ key: loginCredentialsKey, value: null });
    }

    this._authService.login({
      username: credentials.username,
      password: credentials.password
    }).subscribe({
      next: (response) => {
        if (response.errors?.length) {
          this.error = response.errors[0];
        } else {
          this._navigationService.redirectPreLogin();
        }
      },
      error: (err) => {
        this.error = err.error?.errors?.[0] || 'Login failed. Please try again.';
      }
    });
  }
}
