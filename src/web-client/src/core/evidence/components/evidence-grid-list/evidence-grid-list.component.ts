import { Component, input, output } from '@angular/core';
import { PaginatedResult } from '../../../app/type';
import { Evidence } from '../../types';

@Component({
  selector: 'app-evidence-grid-list',
  imports: [],
  templateUrl: './evidence-grid-list.component.html',
  styleUrl: './evidence-grid-list.component.css',
})
/**
 * Component used to render a grid of evidence items
 */
export class EvidenceGridListComponent {
  /**
   * List of items to render in the form of a pagianted evidence
   */
  readonly items = input.required<PaginatedResult<Evidence>>();

  /**
   * Emits a event when a evidence item is selcted from the grid
   */
  readonly itemSelected = output<Evidence>();

  /**
   * Emits a event when there is a paginated response which contains more items that can be fetched - this button will then be displayed - this event
   * is emitted when said button is clciked
   */
  readonly next = output<void>();

  /**
   * Emits a event when there is a paginated response which contains previous items that can be fetched - this button will then be displayed - this event
   * is emitted when said button is clciked
   */
  readonly previous = output<void>();

  /**
   * Holds state of the internally selected evidence item
   */
  _internalSelectedEvidenceItem: Evidence | null = null;

  /**
   * Runs when a item is selected on in the list
   * @param item The evidence item that was clicked
   */
  handleItemClicked(item: Evidence) {
    this._internalSelectedEvidenceItem = item;
    this.itemSelected.emit(this._internalSelectedEvidenceItem);
  }

  /**
   * Runs when a keyboard event is used to click a evidence item
   * @param event The keyboard event
   * @param item The evidence item clicked
   */
  handleItemKeyDownClicked(event: KeyboardEvent, item: Evidence) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.handleItemClicked(item);
    }
  }

  /**
   * Runs when the next button is clicked
   */
  handleNextClicked() {
    if (this.items().hasNextPage) {
      this.next.emit();
    }
  }

  /**
   * Runs when the previous button is clicked
   */
  handlePreviousClicked() {
    if (this.items().hasPreviousPage) {
      this.previous.emit();
    }
  }
}
