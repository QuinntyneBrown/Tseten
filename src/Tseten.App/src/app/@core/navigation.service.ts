// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  lastPath = '/';
  loginUrl = '/login';
  defaultWorkspacePath = '/workspace';

  constructor(private _router: Router) {}

  redirectToLogin(): void {
    this._router.navigate([this.loginUrl]);
  }

  redirectPreLogin(): void {
    const path = this.lastPath !== this.loginUrl ? this.lastPath : this.defaultWorkspacePath;
    this._router.navigate([path]);
  }

  redirectToPublicDefault(): void {
    this._router.navigate(['/']);
  }

  redirectToWorkspace(): void {
    this._router.navigate([this.defaultWorkspacePath]);
  }
}
