import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { PaginatedResult } from '../../app/type';
import { Person, SearchPersonQuery } from '../types';
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
}
