import { Component } from '@angular/core';

@Component({
  selector: 'app-system-incident-type-id-edit-view',
  imports: [],
  templateUrl: './system-incident-type-id-edit-view.component.html',
  styleUrl: './system-incident-type-id-edit-view.component.css'
})
export class SystemIncidentTypeIdEditViewComponent {

  isLoading = true;
  errorMessage:string | null = null;
}
