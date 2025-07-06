import { Component, inject, OnInit } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Tag, TagPagedResult } from '../../types';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { EvidenceService } from '../../services/evidence.service';
import { debounceTime } from 'rxjs';

/**
 * Custom component that used in reactive forms to select mutliple tags in a form group
 */
@Component({
  selector: 'app-search-evidence-tag-multi-select',
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
  ],
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
  implements ControlValueAccessor, OnInit
{
  /**
   * Evidence service used to make calls
   */
  private readonly _evidenceService = inject(EvidenceService);

  /**
   * Form control of the search bar where users type the tag name
   */
  searchTagsInputControl = new FormControl<string | null>('');

  /**
   * Visual list to display for the searched term opf tags
   */
  _searchedTagsResult: TagPagedResult | null = null;

  /**
   * Internal state what current tags are selected
   */
  _internalSelectedTags: Map<string, Tag> = new Map<string, Tag>();



  /**
   * Internal state for any loading state
   */
  _isLoading = false;

  /**
   * Internal error state
   */
  _error: string | null = null;

  ngOnInit(): void {
    this.searchTagsInputControl.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(() => {
        let searchTerm = this.searchTagsInputControl.value;
        if (searchTerm && searchTerm.trim() !== '') {
          this._isLoading = true;
          this._error = null;

          this._evidenceService.searchTags({ name: searchTerm }).subscribe({
            next: (response) => {
              (this._searchedTagsResult = response), (this._isLoading = false);
            },
            error: (err) => {
              this._isLoading = false;
              this._error = 'Failed to fetch tags';
            },
          });
        }
      });
  }

  writeValue(tags: Tag[] | null): void {
    this._internalSelectedTags = new Map<string, Tag>();
    if (tags) {
      tags.forEach((x) => this._internalSelectedTags.set(x.id, x));
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
  }

  onChange: (tags: Tag[]) => void = () => {};
  onTouched: () => void = () => {};

  optionSelected(event: MatAutocompleteSelectedEvent) {
    const selectedTag: Tag = event.option.value;
    this._internalSelectedTags.set(selectedTag.id, selectedTag);
    this.searchTagsInputControl.setValue(null);
    this.onTouched();
    this.onChange(Array.from(this._internalSelectedTags.values()));
  }
}
