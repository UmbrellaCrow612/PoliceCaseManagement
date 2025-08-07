import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AddPersonToCaseDialogComponent } from './components/add-person-to-case-dialog/add-person-to-case-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { PeopleService } from '../../../../core/people/services/people.service';

@Component({
  selector: 'app-cases-id-people-view',
  imports: [MatButtonModule],
  templateUrl: './cases-id-people-view.component.html',
  styleUrl: './cases-id-people-view.component.css',
})
export class CasesIdPeopleViewComponent implements OnInit {
  private readonly dialog = inject(MatDialog);
  private readonly active = inject(ActivatedRoute);
  private readonly peopleService = inject(PeopleService)

  /**
   * The ID of the current case in the UI - extracted from the URL /cases/:caseId
   */
  private caseId: string | null = null;

  /**
   * Holds any error state of the UI
   */
  error: string | null = null;

  /**
   * Runs when add people btn is clicked
   */
  handleAddClicked() {
    this.dialog.open(AddPersonToCaseDialogComponent, {
      data: {
        caseId: this.caseId,
      },
    });
  }

  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
    if (!this.caseId) {
      this.error = 'Failed ot get caseId from URL';
      return;
    }
  }
}
