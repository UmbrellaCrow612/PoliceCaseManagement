import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { CaseAction } from '../type';

@Injectable({
  providedIn: 'root',
})
export class CaseActionService {
  private readonly BASE_URL = env.BaseUrls.casesBaseUrl;

  constructor(private http: HttpClient) {}

  /**
   * Get a case action by it's ID
   * @param caseActionId The case action ID
   */
  getCaseActionById(caseActionId: string) {
    return this.http.get<CaseAction>(
      `${this.BASE_URL}/case-actions/${caseActionId}`
    );
  }
}
