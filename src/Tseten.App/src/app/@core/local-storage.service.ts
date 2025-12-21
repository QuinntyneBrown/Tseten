// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  get({ key }: { key: string }): any {
    const value = localStorage.getItem(key);
    if (value) {
      try {
        return JSON.parse(value);
      } catch {
        return value;
      }
    }
    return null;
  }

  put({ key, value }: { key: string; value: any }): void {
    if (value === null || value === undefined) {
      localStorage.removeItem(key);
    } else {
      const stringValue = typeof value === 'string' ? value : JSON.stringify(value);
      localStorage.setItem(key, stringValue);
    }
  }

  updateLocalStorage(key: string, update: (value: any) => any): void {
    const currentValue = this.get({ key });
    const newValue = update(currentValue);
    this.put({ key, value: newValue });
  }
}
