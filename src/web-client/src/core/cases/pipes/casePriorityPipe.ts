import { Pipe, PipeTransform } from '@angular/core';
import { tryConvertStringToNumber } from '../../app/utils/convert-string-to-number';
import { CasePriorityNames } from '../type';

/**
 * Used to render the text based equal to the number passed to it or unkown if it dose not know it
 */
@Pipe({
    name: 'casePriorityPipe',
})
export class CasePriorityPipe implements PipeTransform {
    transform(value: any): string {
        let number = tryConvertStringToNumber(value)
        if (typeof number !== "number") {
            return "Unkown"
        }

        let found = CasePriorityNames.find(x => x.number === number)?.name
        if (!found) {
            return "Unkown"
        }

        return found;
    }
}