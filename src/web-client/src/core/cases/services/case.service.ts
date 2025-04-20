import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Case, CreateCase } from '../type';

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
}
