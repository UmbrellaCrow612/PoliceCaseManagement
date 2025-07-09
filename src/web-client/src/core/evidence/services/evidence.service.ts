import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Tag, TagPagedResult, UploadEvidence } from '../types';

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

    return this.http.get<TagPagedResult>(urlBuilder.toString());
  }

  /**
   * Upload a piece of evidence - pass it meta data of the file and it will retuns a pre signed URL for the client to upload the evidense file from
   * the client side
   */
  getPreSignedUploadUrl(metaData: UploadEvidence) {
    var urlBuilder = new URL(`${this.BASE_URL}/evidence`);

    Object.entries(metaData).forEach(([key, value]) => {
      if (value === null) {
        return;
      }
      if (value instanceof Date) {
        urlBuilder.searchParams.append(key, value.toISOString());
      } else {
        urlBuilder.searchParams.append(key, value.toString());
      }
    });

    return this.http.get<{ evidenceId: string; uploadUrl: string }>(
      urlBuilder.toString()
    );
  }

  /**
   * Upload a file to amazon s3 from the client side using a pre signed upload URL
   * @param uploadUrl The pre signed URL for this file
   * @param file The file to upload
   */
  uploadFileToS3(uploadUrl: string, file: File) {
    return this.http.put(uploadUrl, file, {
      headers: new HttpHeaders({
        'Content-Type': file.type,
      }),
      reportProgress: true,
      observe: 'events',
    });
  }
}
