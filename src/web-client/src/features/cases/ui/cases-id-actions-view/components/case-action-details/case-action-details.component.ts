import { Component, input } from '@angular/core';
import { CaseAction } from '../../../../../../core/cases/type';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-case-action-details',
  imports: [CommonModule],
  templateUrl: './case-action-details.component.html',
  styleUrl: './case-action-details.component.css'
})
export class CaseActionDetailsComponent {
  details = input.required<CaseAction>()
}
