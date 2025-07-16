import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { BackNavigationButtonComponent } from '../../../../core/components/back-navigation-button/back-navigation-button.component';
import { MatDialog } from '@angular/material/dialog';
import { CreateCaseActionDialogComponent } from './components/create-case-action-dialog/create-case-action-dialog.component';
import { forkJoin, timer } from 'rxjs';
import { CaseService } from '../../../../core/cases/services/case.service';
import { CaseAction, CasePermissionNames } from '../../../../core/cases/type';
import { ActivatedRoute } from '@angular/router';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { CommonModule } from '@angular/common';
import { CaseActionDetailsComponent } from './components/case-action-details/case-action-details.component';
import { getBusinessErrorCode } from '../../../../core/server-responses/getBusinessErrorCode';
import CODES from '../../../../core/server-responses/codes';
import { AuthenticationService } from '../../../../core/authentication/services/authentication.service';

@Component({
  selector: 'app-cases-id-actions-view',
  imports: [
    MatButtonModule,
    BackNavigationButtonComponent,
    CommonModule,
    CaseActionDetailsComponent,
  ],
  templateUrl: './cases-id-actions-view.component.html',
  styleUrl: './cases-id-actions-view.component.css',
})
export class CasesIdActionsViewComponent implements OnInit {
  constructor(
    private caseService: CaseService,
    private active: ActivatedRoute,
    private readonly authService: AuthenticationService
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'Case id missing from URL';
      return;
    }

    this.fetchData();
  }

  @ViewChild('addButtonContainer') addButtonContainer!: ElementRef;

  readonly dialog = inject(MatDialog);
  isLoading = false;
  caseActions: CaseAction[] = [];

  /**
   * Copy of all case permissions in the system
   */
  readonly allPermissions = CasePermissionNames;

  /**
   * Current user permissions they have on the given case
   */
  currentPermissions: string[] = [];
  /**
   * Current case id from the URL
   */
  caseId: string | null = null;
  error: string | null = null;

  addCaseActionClicked() {
    this.dialog
      .open(CreateCaseActionDialogComponent, {
        width: '50%',
        maxWidth: '500px',
        data: {
          caseId: this.caseId,
        },
      })
      .afterClosed()
      .subscribe(() => {
        this.fetchData();
        this.scrollToAddButton();
      });
  }

  fetchData() {
    this.isLoading = true;
    this.error = null;

    if (!this.caseId) {
      return;
    }

    forkJoin([
      this.caseService.getCaseActions(this.caseId),
      this.caseService.getCurrentUsersRoleForCase(this.caseId),
    ]).subscribe({
      next: ([actions, perms]) => {
        this.caseActions = actions;
        this.currentPermissions = perms;

        this.isLoading = false;
        this.scrollToAddButton();
      },
      error: (err) => {
        let code = getBusinessErrorCode(err);

        switch (code) {
          case CODES.CASE_PERMISSION:
            this.authService.UnAuthorized();
            break;

          default:
            this.error = 'Error occured';
            this.isLoading = false;
            break;
        }
      },
    });
  }

  scrollToAddButton() {
    timer(100).subscribe(() => {
      this.addButtonContainer.nativeElement.scrollIntoView({
        behavior: 'smooth',
      });
    });
  }
}
