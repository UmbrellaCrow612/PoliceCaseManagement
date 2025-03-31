import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementEditUserDetailsViewComponent } from './user-management-edit-user-details-view.component';

describe('UserManagementEditUserDetailsViewComponent', () => {
  let component: UserManagementEditUserDetailsViewComponent;
  let fixture: ComponentFixture<UserManagementEditUserDetailsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementEditUserDetailsViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementEditUserDetailsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
