import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AssignUserDialogComponent } from './components/assign-user-dialog/assign-user-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-cases-id-assigned-users',
  imports: [MatButtonModule],
  templateUrl: './cases-id-assigned-users.component.html',
  styleUrl: './cases-id-assigned-users.component.css',
})
export class CasesIdAssignedUsersComponent implements OnInit {
  constructor(
    private readonly dialog: MatDialog,
    private active: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.caseId = this.active.snapshot.paramMap.get('caseId');
  }

  caseId: string | null = null;

  assignUserClicked() {
    this.dialog.open(AssignUserDialogComponent, {
      width: '100%',
      maxWidth: '500px',

      data: {
        currentAssignedUserIds: [], // we pass the currently assigned user to dialog to hide them would also have server val TODO
        caseId: this.caseId,
      },
    });
  }
}
