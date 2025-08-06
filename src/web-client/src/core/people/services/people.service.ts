import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { PaginatedResult } from '../../app/type';
import { CreatePersonDto, Person, SearchPersonQuery } from '../types';
import { Observable } from 'rxjs';

/**
 * Contains all business logic to interact with people / person's in the system
 */
@Injectable({
  providedIn: 'root',
})
export class PeopleService {
  private readonly http = inject(HttpClient);
  private readonly BASE_URL = env.BaseUrls.peopleBaseUrl;

  /**
   * Search for people / person's in the system
   * @returns Paginated response of people
   */
  search(query: SearchPersonQuery): Observable<PaginatedResult<Person>> {
    let urlBuilder = new URL(`${this.BASE_URL}/people/search`);

    for (const [key, value] of Object.entries(query)) {
      if (value === null || value === undefined || value === '') {
        continue;
      }

      if (value instanceof Date) {
        urlBuilder.searchParams.append(key, value.toISOString());
      } else {
        urlBuilder.searchParams.append(key, value);
      }
    }

    return this.http.get<PaginatedResult<Person>>(urlBuilder.toString());
  }

  /**
   * Check if a persons phone number is taken
   * @param phoneNumber The phone number to check
   * @returns Returns a bool to indiacte if the number taken or not
   */
  isPhoneNumberTaken(phoneNumber: string) {
    return this.http.post<{ taken: boolean }>(
      `${this.BASE_URL}/people/phone-numbers/is-taken`,
      {
        phoneNumber,
      }
    );
  }

  /**
   * Check is a persons email is taken by another person in the system
   * @param email The email to check
   * @returns Taken true or false
   */
  isEmailTaken(email: string) {
    return this.http.post<{ taken: boolean }>(
      `${this.BASE_URL}/people/emails/is-taken`,
      {
        email,
      }
    );
  }

  /**
   * Create a person into the system
   * @param personToCreate Details of the person to create
   * @returns HTTP success or failure codes
   */
  create(personToCreate: CreatePersonDto) {
    return this.http.post(`${this.BASE_URL}/people`, { ...personToCreate });
  }
}
