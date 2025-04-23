import { Pipe, PipeTransform } from '@angular/core';
import { tryConvertStringToNumber } from '../../app/utils/convert-string-to-number';
import { CaseStatusNames } from '../type';

/**
 * Used to render the text based equal to the number passed to it or unkown if it dose not know it
 */
@Pipe({
    name: 'caseStatusPipe',
})
export class CaseStatusPipe implements PipeTransform {
    transform(value: any): string {
        let number = tryConvertStringToNumber(value)
        if (typeof number !== "number") {
            return "Unkown"
        }

        let found = CaseStatusNames.find(x => x.number === number)?.name
        if (!found) {
            return "Unkown"
        }

        return found;
    }
}