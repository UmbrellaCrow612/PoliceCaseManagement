import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import env from '../../../environments/environment';
import { IncidentType } from '../types';

@Injectable({
  providedIn: 'root',
})
export class IncidentTypeService {
  constructor(private httpClient: HttpClient) {}

  private BASE_URL = env.BaseUrls.casesBaseUrl;

  /**
   * Retrieve all incident types defined in the system
   * any role can do this.
   */
  getAllIncidentTypes() {
    return this.httpClient.get<Array<IncidentType>>(
      `${this.BASE_URL}/cases/incident-types`
    );
  }

  /**
   * Get details about a incident type by it's id
   */
  getIncidentTypeById(incidentTypeId: string) {
    return this.httpClient.get<IncidentType>(
      `${this.BASE_URL}/cases/incident-types/${incidentTypeId}`
    );
  }

  /**
   * Create a incident type into the system - only admins can do.
   */
  create({
    name,
    description,
  }: Partial<{
    name: string | null;
    description: string | null;
  }>) {
    return this.httpClient.post<IncidentType>(
      `${this.BASE_URL}/cases/incident-types`,
      {
        name,
        description,
      }
    );
  }

  /**
   * Get the amount of times this incident type is linked to x amount of cases.
   */
  getCaseIncidentTypeCount(incidentTypeId: string) {
    return this.httpClient.get<{ count: number }>(
      `${this.BASE_URL}/cases/incident-types/${incidentTypeId}/case-incidents/count`
    );
  }

  /**
   * Delete a incident type by there ID - Only a admin can do this.
   */
  delete(incidentTypeId: string) {
    return this.httpClient.delete(
      `${this.BASE_URL}/cases/incident-types/${incidentTypeId}`
    );
  }

  /**
   * Update a incident type by there.
   */
  update(
    updatedIncidentType: Partial<{
      id: string | null;
      name: string | null;
      description: string | null;
    }>
  ) {
    return this.httpClient.put(
      `${this.BASE_URL}/cases/incident-types/${updatedIncidentType.id}`,
      updatedIncidentType
    );
  }
}
