import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Evidence, SearchEvidenceQuery, Tag, UploadEvidence } from '../types';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../app/type';

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

    return this.http.get<PaginatedResult<Tag>>(urlBuilder.toString());
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

  /**
   * Mark a evidence as uploaded from the client side - we use client side upload - we inform backend that it was uploaded from the client
   * @param evidenceId The evidence that was uploaded from the client
   */
  markEvidenceAsUploaded(evidenceId: string) {
    return this.http.post(
      `${this.BASE_URL}/evidence/${evidenceId}/upload-complete`,
      {}
    );
  }

  /**
   * Search evidence in the system
   * @param query Contains all search query filter options
   * @returns Matching evidence in a paginated format
   */
  searchEvidence(query: SearchEvidenceQuery): Observable<PaginatedResult<Evidence>> {
    var urlBuilder = new URL(`${this.BASE_URL}/evidence/search`);

    Object.entries(query).forEach(([key, value]) => {
      if (value === null || value == undefined) {
        return;
      }
      if (value instanceof Date) {
        urlBuilder.searchParams.append(key, value.toISOString());
      } else {
        urlBuilder.searchParams.append(key, value.toString());
      }
    });
    return this.http.get<PaginatedResult<Evidence>>(urlBuilder.toString());
  }

  /**
   * Get a specific evidence by it's ID
   * @param evidenceId The ID of the evidence to fetch
   * @returns The evidence
   */
  getEvidenceById(evidenceId: string) {
    return this.http.get<Evidence>(`${this.BASE_URL}/evidence/${evidenceId}`);
  }

  /**
   * View a piece of evidencer within the browser without having to download it to the client machine but within there browser
   * @param evidenceId The evidence to view
   * @returns A url used to view a piece of evidence within the browser
   */
  getEvidenceInlineBrowserViewUrl(evidenceId: string) {
    return this.http.get<{ viewUrl: string }>(
      `${this.BASE_URL}/evidence/${evidenceId}/view`
    );
  }
}
