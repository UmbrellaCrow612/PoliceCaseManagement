import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Tag } from '../../types';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { EvidenceService } from '../../services/evidence.service';

/**
 * Custom component that used in reactive forms to select mutliple tags in a form group
 */
@Component({
  selector: 'app-search-evidence-tag-multi-select',
  imports: [MatFormFieldModule, MatInputModule, MatAutocompleteModule],
  templateUrl: './search-evidence-tag-multi-select.component.html',
  styleUrl: './search-evidence-tag-multi-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: SearchEvidenceTagMultiSelectComponent,
      multi: true,
    },
  ],
})
export class SearchEvidenceTagMultiSelectComponent
  implements ControlValueAccessor
{
  @ViewChild('input') input: ElementRef<HTMLInputElement> | null = null;

  /**
   * Form control of the search bar where users type the tag name
   */
  searchTagsInputControl = new FormControl(); // todo add custom asycn validator like case number one and happens on blur

  /**
   * Evidence service used to make calls
   */
  private readonly _evidenceService = inject(EvidenceService);
  /**
   * Internal state how what current tags are selected
   */
  _internalSelectedTags: Tag[] = [];

  /**
   * Internal disabled state
   */
  _isDisabled: boolean = false;

  /**
   * Internal state for any loading state
   */
  _isLoading = false;

  /**
   * Internal error state
   */
  _error: string | null = null;

  writeValue(tags: Tag[]): void {
    this._internalSelectedTags = tags;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this._isDisabled = isDisabled;
  }

  onChange: (tags: Tag[]) => void = () => {};
  onTouched: () => void = () => {};

  optionSelected(event: MatAutocompleteSelectedEvent) {}
}
