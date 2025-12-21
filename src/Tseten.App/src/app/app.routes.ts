// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { authGuard } from './@core';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login').then(m => m.Login)
  },
  {
    path: 'workspace',
    loadComponent: () => import('./pages/workspace').then(m => m.Workspace),
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'requirements',
        pathMatch: 'full'
      },
      {
        path: 'requirements',
        loadComponent: () => import('./pages/requirements-list').then(m => m.RequirementsList)
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
