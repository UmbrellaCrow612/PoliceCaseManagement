import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdAssignedUsersComponent } from './cases-id-assigned-users.component';

describe('CasesIdAssignedUsersComponent', () => {
  let component: CasesIdAssignedUsersComponent;
  let fixture: ComponentFixture<CasesIdAssignedUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdAssignedUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdAssignedUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
