import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdAssignUsersComponent } from './cases-id-assign-users.component';

describe('CasesIdAssignUsersComponent', () => {
  let component: CasesIdAssignUsersComponent;
  let fixture: ComponentFixture<CasesIdAssignUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdAssignUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdAssignUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
