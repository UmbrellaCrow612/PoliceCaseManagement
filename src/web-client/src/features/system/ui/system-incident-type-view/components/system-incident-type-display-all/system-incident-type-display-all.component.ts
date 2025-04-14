import { Component, input, OnInit, output } from '@angular/core';
import { IncidentType } from '../../../../../../core/incident-type/types';
import { MatFormField, MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';

/**
 * Event fired when a incident is clicked
 */
export interface IncidentTypeClickedEvent {
  /**
   * The ID of the incident type - if a string value then a list item if null then no item
   */
  id: string | null;
}

@Component({
  selector: 'app-system-incident-type-display-all',
  imports: [MatInputModule, MatFormField, MatListModule],
  templateUrl: './system-incident-type-display-all.component.html',
  styleUrl: './system-incident-type-display-all.component.css',
})
// Component used to render, filter and select incident types output current select incident type to render more stuff about it
export class SystemIncidentTypeDisplayAllComponent implements OnInit {
  ngOnInit(): void {
    this.filteredIncidentTypes = this.incidentTypes();
  }
  /**
   * Passed incident types fetched
   */
  incidentTypes = input.required<Array<IncidentType>>();

  /**
   * Render of incidentTypes with filter logic
   */
  filteredIncidentTypes: IncidentType[] = [];

  /**
   * Currently clicked incident type tracked for displaying extra styles
   */
  internalCurrentSelectedIncidentTypeId: string | null = null;

  /**
   * Event emitted and passes the clicked incident type event
   */
  clickedIncidentTypeEvent = output<IncidentTypeClickedEvent>();

  /**
   * Filter the incident types passed in
   */
  onSearchInputChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.value.trim() !== '') {
      this.filteredIncidentTypes = this.incidentTypes().filter((x) =>
        x.name.toLowerCase().includes(input.value.toLocaleLowerCase().trim())
      );
    } else {
      this.filteredIncidentTypes = this.incidentTypes();
    }
    event.preventDefault();
  }

  /**
   * Logic to run when a incident type list item is clicked
   */
  listItemClicked(clickedIncidentType: IncidentType) {
    if (this.internalCurrentSelectedIncidentTypeId) {
      if (
        clickedIncidentType.id === this.internalCurrentSelectedIncidentTypeId
      ) {
        // here they clicked the same item so they de select it
        this.internalCurrentSelectedIncidentTypeId = null;
      } else {
        this.internalCurrentSelectedIncidentTypeId = clickedIncidentType.id;
      }
    } else {
      this.internalCurrentSelectedIncidentTypeId = clickedIncidentType.id;
    }

    this.clickedIncidentTypeEvent.emit({
      id: this.internalCurrentSelectedIncidentTypeId,
    });
  }
}
