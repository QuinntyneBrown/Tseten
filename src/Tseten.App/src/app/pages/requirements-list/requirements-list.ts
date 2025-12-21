// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { SoftwareRequirement } from '../../models';

@Component({
  selector: 'app-requirements-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './requirements-list.html',
  styleUrl: './requirements-list.scss'
})
export class RequirementsList implements OnInit {
  private _http = inject(HttpClient);
  private _baseUrl = environment.baseUrl;

  requirements: SoftwareRequirement[] = [];
  loading = true;
  error = '';

  ngOnInit(): void {
    this.loadRequirements();
  }

  loadRequirements(): void {
    this.loading = true;
    this._http.get<{ softwareRequirements: SoftwareRequirement[] }>(
      `${this._baseUrl}/api/softwarerequirements`
    ).subscribe({
      next: (response) => {
        this.requirements = response.softwareRequirements || [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load requirements';
        this.loading = false;
      }
    });
  }
}
