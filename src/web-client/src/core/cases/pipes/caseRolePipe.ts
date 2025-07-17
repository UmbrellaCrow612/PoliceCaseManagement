import { Pipe, PipeTransform } from '@angular/core';
import { CaseRoleNameMap, CaseRoleValue } from '../type';

/**
 * Custom pipe to render the CaseRole to a string name to render it in the UI
 */
@Pipe({
  name: 'caseRolePipe',
})
export class CaseRolePipe implements PipeTransform {
  transform(value: CaseRoleValue): string {
    return CaseRoleNameMap[value] ?? 'Unknown Role';
  }
}
