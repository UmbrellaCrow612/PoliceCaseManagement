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
import { timer } from 'rxjs';
import { CaseService } from '../../../../core/cases/services/case.service';
import { CaseAction } from '../../../../core/cases/type';
import { ActivatedRoute } from '@angular/router';
import { ErrorService } from '../../../../core/app/errors/services/error.service';
import { CommonModule } from '@angular/common';
import { CaseActionDetailsComponent } from './components/case-action-details/case-action-details.component';

@Component({
  selector: 'app-cases-id-actions-view',
  imports: [MatButtonModule, BackNavigationButtonComponent, CommonModule, CaseActionDetailsComponent],
  templateUrl: './cases-id-actions-view.component.html',
  styleUrl: './cases-id-actions-view.component.css',
})
export class CasesIdActionsViewComponent implements AfterViewInit, OnInit {
  constructor(
    private caseService: CaseService,
    private active: ActivatedRoute,
    private errorService: ErrorService
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'Case id missing from URL';
      return;
    }

    this.fetchData();
  }
  ngAfterViewInit(): void {
    this.scrollToAddButton();
  }

  @ViewChild('addButtonContainer') addButtonContainer!: ElementRef;

  readonly dialog = inject(MatDialog);
  isLoading = false;
  caseActions: CaseAction[] = [];

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

    this.caseService.getCaseActions(this.caseId).subscribe({
      next: (response) => {
        this.caseActions = response;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Error occured';
        this.errorService.HandleDisplay(err);
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
