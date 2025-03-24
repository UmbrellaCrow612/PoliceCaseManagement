import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementEditUserViewComponent } from './user-management-edit-user-view.component';

describe('UserManagementEditUserViewComponent', () => {
  let component: UserManagementEditUserViewComponent;
  let fixture: ComponentFixture<UserManagementEditUserViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementEditUserViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementEditUserViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
