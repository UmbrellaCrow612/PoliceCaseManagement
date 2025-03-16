import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { rolesAuthorizationGuard } from './roles-authorization.guard';

describe('rolesAuthorizationGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => rolesAuthorizationGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
