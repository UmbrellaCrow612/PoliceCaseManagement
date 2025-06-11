import { Component, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CasePriorityNames } from '../../type';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-case-priority-select',
  imports: [MatFormFieldModule, MatSelectModule],
  templateUrl: './case-priority-select.component.html',
  styleUrl: './case-priority-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CasePrioritySelectComponent),
      multi: true,
    },
  ],
})
export class CasePrioritySelectComponent implements ControlValueAccessor {
  writeValue(value: number | null): void {
    this.selectedValue = value;
  }
  registerOnChange(fn: (value: number | null) => void): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }

  onChange: (value: number | null) => void = () => {};

  onTouched: () => void = () => {};

  /**
   * Used to disable the component to stop changes
   */
  isDisabled = false;

  /**
   * Local copy of case prioertiys to be used in the UI
   */
  casePriorityNames = CasePriorityNames;

  /**
   * Internal value for the select component.
   */
  selectedValue: number | null = null;

  onSelectionChange(event: MatSelectChange): void {
    this.selectedValue = event.value;
    this.onChange(this.selectedValue);
    this.onTouched();
  }

  onOpenedChange(opened: boolean): void {
    if (!opened) {
      this.onTouched();
    }
  }
}
