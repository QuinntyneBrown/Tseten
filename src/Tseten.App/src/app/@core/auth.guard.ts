// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LocalStorageService } from './local-storage.service';
import { NavigationService } from './navigation.service';
import { accessTokenKey } from './storage-keys';

export const authGuard: CanActivateFn = (route, state) => {
  const localStorageService = inject(LocalStorageService);
  const navigationService = inject(NavigationService);
  const router = inject(Router);

  const accessToken = localStorageService.get({ key: accessTokenKey });

  if (accessToken) {
    return true;
  }

  navigationService.lastPath = state.url;
  navigationService.redirectToLogin();
  return false;
};
