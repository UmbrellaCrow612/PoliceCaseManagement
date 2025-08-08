// kebab-case.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';
import { CasePersonRoleNames } from '../types';

/**
 * Pipe used ot render the string name of a cas eperson role enum value
 */
@Pipe({
  name: 'casePersonRole',
})
export class CasePersonRole implements PipeTransform {
  transform(value: number): string {
    let found = CasePersonRoleNames.find((x) => x.value == value);
    if (!found) {
      return 'Not found';
    }

    return found.name;
  }
}
