// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, ReplaySubject, map, tap } from 'rxjs';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, currentUserKey } from './storage-keys';
import { environment } from '../../environments/environment';

export interface User {
  userId: string;
  username: string;
  defaultProfileId?: string;
  roles: Role[];
}

export interface Role {
  roleId: string;
  name: string;
  privileges: Privilege[];
}

export interface Privilege {
  privilegeId: string;
  roleId: string;
  aggregate: string;
  accessRight: AccessRight;
}

export enum AccessRight {
  None = 0,
  Read = 1,
  Write = 2,
  Create = 3,
  Delete = 4
}

export interface AuthenticateResponse {
  userId: string;
  token: string;
  refreshToken: string;
  errors?: string[];
}

export interface LoginCredentials {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _http = inject(HttpClient);
  private _localStorageService = inject(LocalStorageService);
  private _currentUserSubject = new ReplaySubject<User | null>(1);

  readonly baseUrl = environment.baseUrl;
  readonly currentUser$ = this._currentUserSubject.asObservable();

  login(options: { username: string; password: string }): Observable<AuthenticateResponse> {
    return this._http.post<AuthenticateResponse>(`${this.baseUrl}/api/user/token`, options)
      .pipe(
        tap(response => {
          if (response.token) {
            this._localStorageService.put({ key: accessTokenKey, value: response.token });
          }
        })
      );
  }

  logout(): void {
    this._localStorageService.put({ key: accessTokenKey, value: null });
    this._localStorageService.put({ key: currentUserKey, value: null });
    this._currentUserSubject.next(null);
  }

  tryToLogin(): Observable<User> {
    return this._http.get<{ user: User }>(`${this.baseUrl}/api/user/current`)
      .pipe(
        map(response => response.user),
        tap(user => {
          this._currentUserSubject.next(user);
          this._localStorageService.put({ key: currentUserKey, value: user });
        })
      );
  }

  setCurrentUser(user: User | null): void {
    this._currentUserSubject.next(user);
    if (user) {
      this._localStorageService.put({ key: currentUserKey, value: user });
    }
  }

  hasReadWritePrivileges$(aggregate: string): Observable<boolean> {
    return this.currentUser$.pipe(
      map(user => {
        if (!user) return false;

        const hasRead = this._hasPrivilege(user, aggregate, AccessRight.Read);
        const hasWrite = this._hasPrivilege(user, aggregate, AccessRight.Write);

        return hasRead && hasWrite;
      })
    );
  }

  private _hasPrivilege(user: User, aggregate: string, accessRight: AccessRight): boolean {
    return user.roles.some(role =>
      role.privileges.some(p =>
        p.aggregate === aggregate && p.accessRight === accessRight
      )
    );
  }

  isAuthenticated(): boolean {
    const token = this._localStorageService.get({ key: accessTokenKey });
    return !!token;
  }
}
