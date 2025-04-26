import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CasePriorityNames, CaseStatusNames } from '../../../../core/cases/type';

@Component({
  selector: 'app-search-cases-view',
  imports: [ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, CommonModule,MatSelectModule],
  providers: [provideNativeDateAdapter()],
  templateUrl: './search-cases-view.component.html',
  styleUrl: './search-cases-view.component.css'
})
export class SearchCasesViewComponent {


  searchCasesFrom = new FormGroup({
    caseNumber: new FormControl<string | null>(null),
    incidentDateTime: new FormControl<Date | null>(null),
    reportedDateTime: new FormControl<Date | null>(null),
    status: new FormControl<number | null>(null),
    priority: new FormControl<number | null>(null),
    reportingOfficerUserName: new FormControl<string | null>(null)
  })


  caseStatus = CaseStatusNames
  casePrioritys = CasePriorityNames


  onSubmit(){}
}
