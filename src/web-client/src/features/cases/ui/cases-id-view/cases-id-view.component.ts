import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CaseService } from '../../../../core/cases/services/case.service';
import { Case } from '../../../../core/cases/type';
import { HttpErrorResponse } from '@angular/common/http';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { CommonModule } from '@angular/common';
import { CasePriorityPipe } from '../../../../core/cases/pipes/casePriorityPipe';
import { CaseStatusPipe } from '../../../../core/cases/pipes/caseStatusPipe';

@Component({
  selector: 'app-cases-id-view',
  imports: [CommonModule, CasePriorityPipe, CaseStatusPipe],
  templateUrl: './cases-id-view.component.html',
  styleUrl: './cases-id-view.component.css'
})
export class CasesIdViewComponent implements OnInit {

  constructor(private active: ActivatedRoute, private caseService: CaseService, private errorService: ErrorService) { }
  ngOnInit(): void {
    this.isLoading = true;
    this.errorMessage = null;


    this.caseId = this.active.snapshot.paramMap.get("caseId")
    if (!this.caseId) {
      this.errorMessage = "Failed to get case ID from URL"
      this.isLoading = false
      return;
    }

    this.fetchData()
  }

  caseId: string | null = null;
  isLoading = true;
  errorMessage: string | null = null;
  caseDetails: Case | null = null;


  fetchData() {
    if (!this.caseId) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;


    this.caseService.getCaseById(this.caseId).subscribe({
      next: (response) => {
        this.caseDetails = response
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        let code = getBusinessErrorCode(err)
        this.isLoading = false;
        this.errorMessage = "Failed"

        switch (code) {
          case "dw":

            break;

          default:
            this.errorService.HandleDisplay(err)
            break;
        }
      }
    })
  }
}
