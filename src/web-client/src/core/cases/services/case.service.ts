import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import {
  Case,
  CaseAction,
  CaseAttachment,
  CasePagedResult,
  CreateCase,
  CreateCaseAction,
} from '../type';
import { IncidentType } from '../../incident-type/types';
import { CaseUser, RestrictedUser } from '../../user/type';

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
   * Get all case actions for a given case
   * @param caseId The case ID
   * @returns Observable
   */
  getCaseActions(caseId: string) {
    return this.httpClient.get<CaseAction[]>(
      `${this.BASE_URL}/cases/${caseId}/case-actions`
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

  /**
   * Add a case action to a given case by it's ID
   * @param caseId The case id of the case to add this case action to
   * @param caseActionToCreate Contains the information of the action to create
   * @returns Observable
   */
  addCaseAction(caseId: string, caseActionToCreate: CreateCaseAction) {
    return this.httpClient.post<CaseAction>(
      `${this.BASE_URL}/cases/${caseId}/case-actions`,
      { ...caseActionToCreate }
    );
  }

  /**
   * Assign a set of users to a case
   * @param usersToAssign List of users to assign to this case - make sure there unquie and do not contain already assigned users to the given case, will be validated on server anyways
   * @param caseId The case to link to
   */
  assignUsersToCase(caseId: string, usersToAssign: RestrictedUser[]) {
    return this.httpClient.post(`${this.BASE_URL}/cases/${caseId}/users`, {
      userIds: usersToAssign.map((x) => x.id),
    });
  }

  /**
   * Get all users assigned to a given case.
   * @param caseId The ID of the case
   * @returns List of users assigned to the given case
   */
  getAssignedUsers(caseId: string) {
    return this.httpClient.get<CaseUser[]>(
      `${this.BASE_URL}/cases/${caseId}/users`
    );
  }

  /**
   * Remove a assigned user from a case
   * @param caseId The ID of the case
   * @param userId The ID of he user
   */
  removeUser(caseId: string, userId: string) {
    return this.httpClient.delete(
      `${this.BASE_URL}/cases/${caseId}/users/${userId}`
    );
  }

  /**
   * Get all case attachments for a given case
   * @param caseId The ID of the case to get the files for
   * @returns List of attachments
   */
  getAttachments(caseId: string) {
    return this.httpClient.get<CaseAttachment[]>(
      `${this.BASE_URL}/cases/${caseId}/attachments`
    );
  }

  /**
   * Upload a case file attachament to a given case
   * @param caseId The case to add it to
   * @param file The file
   */
  uploadAttachment(caseId: string, file: File) {
    const formData = new FormData();
    formData.append('file', file); // Must match parameter name in controller

    return this.httpClient.post(
      `${this.BASE_URL}/cases/${caseId}/attachments/upload`,
      formData
    );
  }

  /**
   * Download a specific case attachment
   * @param attachamentId The attachment id to download
   * @returns Stream blob
   */
  downloadAttachment(attachamentId: string) {
    return this.httpClient.get(
      `${this.BASE_URL}/cases/attachments/download/${attachamentId}`,
      {
        responseType: 'blob',
        observe: 'response',
      }
    );
  }
}
