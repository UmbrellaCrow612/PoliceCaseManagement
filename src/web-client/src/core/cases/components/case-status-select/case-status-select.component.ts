import { Component, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CaseStatusNames } from '../../type';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-case-status-select',
  imports: [MatFormFieldModule, MatSelectModule],
  templateUrl: './case-status-select.component.html',
  styleUrl: './case-status-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CaseStatusSelectComponent),
      multi: true,
    },
  ],
})
export class CaseStatusSelectComponent implements ControlValueAccessor {
  /**
   * Writes a new value to the element.
   * This method is called by the Forms API to update the view when the form model changes.
   */
  writeValue(value: number | null): void {
    this.selectedValue = value;
  }

  /**
   * Registers a callback function that should be called when the control's value changes in the UI.
   */
  registerOnChange(fn: (value: number | null) => void): void {
    this.onChange = fn;
  }

  /**
   * Registers a callback function that should be called when the control receives a touch event.
   */
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  /**
   * This function is called by the Forms API when the control's disabled state changes.
   */
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  /**
   * Used to disable the component to stop changes
   */
  isDisabled = false;

  /**
   * Local copy of case status to be used in the UI
   */
  caseStatusNames = CaseStatusNames;

  /**
   * Internal value for the select component.
   */
  selectedValue: number | null = null;

  onChange: (value: number | null) => void = () => {};

  onTouched: () => void = () => {};

  /**
   * Handles the selection change event from the mat-select.
   * @param event The MatSelectChange event.
   */
  onSelectionChange(event: MatSelectChange): void {
    this.selectedValue = event.value;
    this.onChange(this.selectedValue);
    this.onTouched();
  }

  /**
   * Handles the openedChange event to mark the control as touched when the dropdown is closed.
   * This is a common pattern for "touched" state with selects.
   */
  onOpenedChange(opened: boolean): void {
    if (!opened) {
      this.onTouched();
    }
  }
}
