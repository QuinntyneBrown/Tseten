// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink } from '@angular/router';
import { AuthService, NavigationService, User } from '../../@core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-workspace',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  templateUrl: './workspace.html',
  styleUrl: './workspace.scss'
})
export class Workspace implements OnInit {
  private _authService = inject(AuthService);
  private _navigationService = inject(NavigationService);

  currentUser$!: Observable<User | null>;

  ngOnInit(): void {
    this.currentUser$ = this._authService.currentUser$;
    this._authService.tryToLogin().subscribe();
  }

  logout(): void {
    this._authService.logout();
    this._navigationService.redirectToPublicDefault();
  }
}
