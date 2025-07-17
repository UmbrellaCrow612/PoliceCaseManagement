import { Pipe, PipeTransform } from '@angular/core';
import { CaseRoleNameMap, CaseRoleValue } from '../type';

/**
 * Custom pipe to render the CaseRole to a string name in the UI - for example if there case role is 0 it would then render the string "Owner" or 
 * "Unknown Role" if it cannot find it
 */
@Pipe({
  name: 'caseRolePipe',
})
export class CaseRolePipe implements PipeTransform {
  transform(value: CaseRoleValue): string {
    return CaseRoleNameMap[value] ?? 'Unknown Role';
  }
}
