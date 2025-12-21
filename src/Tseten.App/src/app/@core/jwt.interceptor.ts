// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { LocalStorageService } from './local-storage.service';
import { NavigationService } from './navigation.service';
import { accessTokenKey } from './storage-keys';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const localStorageService = inject(LocalStorageService);
  const navigationService = inject(NavigationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        localStorageService.put({ key: accessTokenKey, value: null });
        navigationService.redirectToLogin();
      }
      return throwError(() => error);
    })
  );
};
