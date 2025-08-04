import { Component, OnInit } from '@angular/core';
import { IncidentTypeService } from '../../../../../core/incident-type/services/incident-type-service.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IncidentType } from '../../../../../core/incident-type/types';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-system-incident-type-home-view',
  imports: [
    MatButtonModule,
    MatFormField,
    MatInputModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    RouterModule
  ],
  templateUrl: './system-incident-type-home-view.component.html',
  styleUrl: './system-incident-type-home-view.component.css',
})
export class SystemIncidentTypeHomeViewComponent implements OnInit {
  constructor(private incidentTypeService: IncidentTypeService) {}

  ngOnInit(): void {
    this.fetchData();

    this.searchForm.valueChanges.subscribe(() => {
      const nameSearchTerm = this.searchForm.controls.name
        .getRawValue()
        ?.toLowerCase();
      const descriptionSearchTerm = this.searchForm.controls.description
        .getRawValue()
        ?.toLowerCase();

      this.filteredIncidentTypes = this.incidentTypes.filter((x) => {
        const matchesName = nameSearchTerm
          ? x.name?.toLowerCase().includes(nameSearchTerm)
          : true;

        const matchesDescription = descriptionSearchTerm
          ? x.description?.toLowerCase().includes(descriptionSearchTerm)
          : true;

        return matchesName && matchesDescription;
      });
    });
  }

  isLoading = true;
  errorMessage: string | null = null;

  searchForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
  });

  incidentTypes: IncidentType[] = [];
  filteredIncidentTypes: IncidentType[] = [];

  fetchData() {
    this.isLoading = true;
    this.errorMessage = null;

   
  }
}
