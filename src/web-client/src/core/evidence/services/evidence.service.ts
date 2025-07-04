import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Tag } from '../types';

/**
 * Central service to do all Evidence related business logic
 */
@Injectable({
  providedIn: 'root',
})
export class EvidenceService {
  private readonly BASE_URL = env.BaseUrls.evidenceBaseUrl;
  private readonly http = inject(HttpClient);

  /**
   * Check to see if a given reference number has already been used for another piece of evidence as they are unique
   * @param referenceNumber The number to check
   * @returns HTTP Observable if it returns 200 it means it is not taken any other status code it has
   */
  isReferenceNumberTaken(referenceNumber: string) {
    return this.http.post<{ isTaken: boolean }>(
      `${this.BASE_URL}/evidence/reference-numbers/is-taken`,
      {
        referenceNumber: referenceNumber,
      }
    );
  }

  /**
   * Search for tags in the system
   * @param searchQuery A query object to search by
   * @returns List of tags matching the criteria
   */
  searchTags(searchQuery: { name: string }) {
    let urlBuilder = new URL(`${this.BASE_URL}/evidence/tags`);

    urlBuilder.searchParams.append('name', searchQuery.name);

    return this.http.get<Tag[]>(urlBuilder.toString());
  }
}
