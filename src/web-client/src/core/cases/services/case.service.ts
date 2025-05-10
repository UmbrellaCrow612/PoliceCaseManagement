import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Case, CasePagedResult, CreateCase } from '../type';
import { IncidentType } from '../../incident-type/types';

@Injectable({
  providedIn: 'root',
})
export class CaseService {
  constructor(private httpClient: HttpClient) {}

  private readonly BASE_URL = env.BaseUrls.casesBaseUrl;

  /**
   * Create a case
   * @param caseToCreate case to create
   * @returns Observable and created case
   */
  create(caseToCreate: CreateCase) {
    return this.httpClient.post<Case>(`${this.BASE_URL}/cases`, {
      ...caseToCreate,
    });
  }

  /**
   * Check if a case number is taken already
   * @param caseNumber The case number
   * @returns 200 if it isnt other if it is
   */
  isCaseNumberTaken(caseNumber: string) {
    return this.httpClient.get(
      `${this.BASE_URL}/cases/case-numbers/${caseNumber}/is-taken`
    );
  }

  /**
   * Get a case by it's ID
   * @param caseId case ID
   * @returns The case details or error if ot found.
   */
  getCaseById(caseId: string) {
    return this.httpClient.get<Case>(`${this.BASE_URL}/cases/${caseId}`);
  }

  /**
   * Search for cases in the system
   * @param options Search params to append to the URL and filter cases based on them
   * @returns Observale and a paged result
   */
  searchCasesWithPagination(
    options: Partial<{
      caseNumber: string | null;
      incidentDateTime: Date | null;
      reportedDateTime: Date | null;
      status: string | null;
      priority: string | null;
      reportingOfficerId: string | null;
      incidentTypeId: string | null;
      pageNumber: number | null;
    }>
  ) {
    let urlBuilder = new URL(`${this.BASE_URL}/cases/search`);

    if (options.caseNumber) {
      urlBuilder.searchParams.append('caseNumber', options.caseNumber);
    }

    if (options.incidentDateTime) {
      const incidentDate = new Date(options.incidentDateTime);
      urlBuilder.searchParams.append(
        'incidentDateTime',
        incidentDate.toUTCString()
      );
    }

    if (options.reportedDateTime) {
      const reportedDate = new Date(options.reportedDateTime);
      urlBuilder.searchParams.append(
        'reportedDateTime',
        reportedDate.toUTCString()
      );
    }

    if (options.status) {
      urlBuilder.searchParams.append('status', options.status.toString());
    }

    if (options.priority) {
      urlBuilder.searchParams.append('priority', options.priority.toString());
    }

    if (options.reportingOfficerId) {
      urlBuilder.searchParams.append(
        'reportingOfficerId',
        options.reportingOfficerId
      );
    }

    if (options.incidentTypeId) {
      urlBuilder.searchParams.append('incidentTypeId', options.incidentTypeId);
    }

    if (options.pageNumber) {
      urlBuilder.searchParams.append(
        'pageNumber',
        options.pageNumber.toString()
      );
    }

    let url = urlBuilder.toString();

    return this.httpClient.get<CasePagedResult>(url);
  }

  /**
   * Get all the incident types linked to a given case.
   * @param caseId The case id
   * @returns Observable list of incident types
   */
  getIncidentTypes(caseId: string) {
    return this.httpClient.get<IncidentType[]>(
      `${this.BASE_URL}/cases/${caseId}/incident-types`
    );
  }

  /**
   * Update the incident types a given case is related to
   * @param caseId The ID of the case you want to update the inciden types for
   * @param incidentTypeIds A array of incident types you want to link to it - contain new and old incident types
   * @returns Observable
   */
  updateIncidentTypes(caseId: string, incidentTypeIds: string[]) {
    return this.httpClient.put(
      `${this.BASE_URL}/cases/${caseId}/incident-types`,
      {
        incidentTypeIds: incidentTypeIds,
      }
    );
  }
}
