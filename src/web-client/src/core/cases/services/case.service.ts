import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { Case, CreateCase } from '../type';

@Injectable({
  providedIn: 'root',
})
export class CaseService {
  constructor(private httpClient: HttpClient) { }

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
  isCaseNumberTaken(caseNumber:string) {
    return this.httpClient.get(`${this.BASE_URL}/cases/case-numbers/${caseNumber}/is-taken`)
  }



  /**
   * Get a case by it's ID
   * @param caseId case ID
   * @returns The case details or error if ot found.
   */
  getCaseById(caseId:string){
    return this.httpClient.get<Case>(`${this.BASE_URL}/cases/${caseId}`)
  }
}
