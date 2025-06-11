import { Component, input, output } from '@angular/core';
import { CasePagedResult } from '../../type';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CaseStatusPipe } from '../../pipes/caseStatusPipe';
import { CasePriorityPipe } from '../../pipes/casePriorityPipe';

@Component({
  selector: 'app-case-grid-list',
  imports: [MatListModule, MatButtonModule, RouterModule, CommonModule, CaseStatusPipe, CasePriorityPipe],
  templateUrl: './case-grid-list.component.html',
  styleUrl: './case-grid-list.component.css',
})
/**
 * Used to render a list of cases from a search query
 */
export class CaseGridListComponent {
  /**
   * Pages result of cases to render
   */
  casePagedResult = input.required<CasePagedResult>();

  /**
   * Event emitted when there is a next button rendered and it is clicked
   */
  nextClicked = output();

  /**
   * Event fired off when the previous button is visible and it is clicked
   */
  previousClicked = output();
}
