import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink, RouterOutlet } from '@angular/router';
import { CaseService } from '../../../../core/cases/services/case.service';
import { Case, CaseRoleNames } from '../../../../core/cases/type';
import { CommonModule } from '@angular/common';
import { CasePriorityPipe } from '../../../../core/cases/pipes/casePriorityPipe';
import { CaseStatusPipe } from '../../../../core/cases/pipes/caseStatusPipe';
import { MatButtonModule } from '@angular/material/button';
import { forkJoin } from 'rxjs';
import { IncidentType } from '../../../../core/incident-type/types';
import { AppLink } from '../../../../core/app/type';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';
import CODES from '../../../../core/server-responses/codes';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';

@Component({
  selector: 'app-cases-id-view',
  imports: [
    CommonModule,
    CasePriorityPipe,
    CaseStatusPipe,
    MatButtonModule,
    RouterLink,
  ],
  templateUrl: './cases-id-view.component.html',
  styleUrl: './cases-id-view.component.css',
})
export class CasesIdViewComponent implements OnInit {
  constructor(
    private active: ActivatedRoute,
    private caseService: CaseService,
    private readonly authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.errorMessage = 'Failed to get case ID from URL';
      this.isLoading = false;
      return;
    }

    this.fetchData();
  }

  caseId: string | null = null;
  isLoading = true;
  errorMessage: string | null = null;
  caseDetails: Case | null = null;
  incidentTypes: IncidentType[] = [];

  copy_CaseRoleName = CaseRoleNames;

  /**
   * The users current role they have on the given case
   */
  currentUserCaseRole: number | null = null;

  fetchData() {
    if (!this.caseId) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    forkJoin([
      this.caseService.getCaseById(this.caseId),
      this.caseService.getIncidentTypes(this.caseId),
      this.caseService.getCurrentUsersRoleForCase(this.caseId),
    ]).subscribe({
      next: ([caseDetails, incidentTypes, currentRole]) => {
        this.caseDetails = caseDetails;
        this.incidentTypes = incidentTypes;
        this.currentUserCaseRole = currentRole;
        this.isLoading = false;
      },
      error: (err) => {
        let code = getBusinessErrorCode(err);

        switch (code) {
          case CODES.CASE_PERMISSION:
            this.authService.UnAuthorized();
            break;

          default:
            this.errorMessage = 'Could not find case';
            break;
        }

        this.isLoading = false;
      },
    });
  }

  links: AppLink[] = [
    {
      authorizedRoles: [],
      href: './actions',
      name: 'Actions',
    },
    {
      authorizedRoles: [],
      href: './assigned-users',
      name: 'Assigned Officers',
    },
    {
      authorizedRoles: [],
      href: './attachments',
      name: 'File attachments',
    },
  ];
}
