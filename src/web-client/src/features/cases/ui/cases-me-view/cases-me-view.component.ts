import { Component, inject } from '@angular/core';
import { UserService } from '../../../../core/user/services/user.service';
import { CaseService } from '../../../../core/cases/services/case.service';

@Component({
  selector: 'app-cases-me-view',
  imports: [],
  templateUrl: './cases-me-view.component.html',
  styleUrl: './cases-me-view.component.css',
})
export class CasesMeViewComponent {
  private user = inject(UserService).USER;
  private readonly caseService = inject(CaseService);


  /**
   * Used to filter cases for the current user still using the search endpoint but pre filling it with the current users details
   */
  private searchMyCasesQuery = {}

  // we dont add it to urls here it will be added internal;ly in the on init of the view
}
