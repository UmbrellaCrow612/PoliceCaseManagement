import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementEditUserRolesViewComponent } from './user-management-edit-user-roles-view.component';

describe('UserManagementEditUserRolesViewComponent', () => {
  let component: UserManagementEditUserRolesViewComponent;
  let fixture: ComponentFixture<UserManagementEditUserRolesViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementEditUserRolesViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementEditUserRolesViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
