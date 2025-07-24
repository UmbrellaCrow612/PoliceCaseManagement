import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import {
  Case,
  CaseAction,
  CaseAttachment,
  CasePagedResult,
  CaseRoleValue,
  CreateCase,
  CreateCaseAction,
} from '../type';
import { IncidentType } from '../../incident-type/types';
import { CaseAcessList, RestrictedUser } from '../../user/type';
import { CaseEvidence, Evidence } from '../../evidence/types';

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
      assignedUserIds: string[];
      createdById: string | null;
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
        incidentDate.toISOString()
      );
    }

    if (options.reportedDateTime) {
      const reportedDate = new Date(options.reportedDateTime);
      urlBuilder.searchParams.append(
        'reportedDateTime',
        reportedDate.toISOString()
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

    if (options.assignedUserIds && options.assignedUserIds.length > 0) {
      options.assignedUserIds.forEach((x) => {
        urlBuilder.searchParams.append('assignedUserIds', x);
      });
    }

    if (options.createdById) {
      urlBuilder.searchParams.append('createdById', options.createdById);
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
   * Assign a given user to a case
   * @param caseId The case to assign it to
   * @param user The user to assign
   * @returns Result pattern
   */
  assignUserToCase(caseId: string, user: RestrictedUser) {
    return this.httpClient.post(`${this.BASE_URL}/cases/${caseId}/users`, {
      userId: user.id,
    });
  }

  /**
   * Get all users assigned to a given case.
   * @param caseId The ID of the case
   * @returns List of users assigned to the given case in the form of a access list
   */
  getAssignedUsers(caseId: string) {
    return this.httpClient.get<CaseAcessList[]>(
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
   * Get a pre signed URL to upload a specific ase file attachment to a case client side upload
   * @param caseId The case to upload the file ot
   * @param file The JavaScript File object
   * @param newFileName A optional new name for the file instead of the JavaScript Object File File.name
   * @returns Presigned URL response for the given file
   */
  getPreSignedUrlForCaseAttachmentFile(
    caseId: string,
    file: File,
    newFileName: string | null = null
  ) {
    return this.httpClient.post<{
      fileId: string;
      uploadUrl: string;
    }>(`${this.BASE_URL}/cases/${caseId}/attachments/upload`, {
      contentType: file.type,
      fileName: newFileName ? newFileName : file.name,
      fileSize: file.size,
    });
  }

  /**
   * Upload a file using a pre signed URL
   * @param preSignedUrl A pre signed URL for the given file
   * @param file The file to upload
   * @returns HTTP response
   */
  uploadUsingPreSignedUrl(preSignedUrl: string, file: File) {
    return this.httpClient.put(preSignedUrl, file, {
      headers: new HttpHeaders({
        'Content-Type': file.type,
      }), // changed this bit observer
    });
  }

  /**
   * Used to mark a case attachment file as uploaded succesfully fgrom the client side
   * @param attachmentId The file attachment ID
   * @returns HTTP Response
   */
  confirmUploadOfCaseAttachmentFile(attachmentId: string) {
    return this.httpClient.post(
      `${this.BASE_URL}/attachments/${attachmentId}/complete`,
      {}
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

  /**
   * Delete a specific case attachament by it's ID
   * @param attachamentId The file to delete
   */
  deleteAttachment(attachamentId: string) {
    return this.httpClient.delete(
      `${this.BASE_URL}/cases/attachments/${attachamentId}`
    );
  }

  /**
   * Get the current users role for a given case - it will get the current context logeed in users role for the given case
   * @param caseId The Case to get the current requesting users permission for
   * @returns The role - enum number
   */
  getCurrentUsersRoleForCase(caseId: string) {
    return this.httpClient.get<{ role: CaseRoleValue }>(
      `${this.BASE_URL}/cases/${caseId}/me`
    );
  }

  /**
   * Get all linked evidence for a given case
   * @param caseId The ID of the case to fetch evidence for
   * @returns List of case evidence links
   */
  getEvidence(caseId: string) {
    return this.httpClient.get<CaseEvidence[]>(
      `${this.BASE_URL}/cases/${caseId}/evidence`
    );
  }

  /**
   * Link a piece of evidence to a given case
   * @param caseId The case to link for
   * @param evidenceId The evidence to link
   * @returns Result object or success
   */
  addEvidence(caseId: string, evidenceId: string) {
    return this.httpClient.post(`${this.BASE_URL}/cases/${caseId}/evidence`, {
      evidenceId: evidenceId,
    });
  }
}
