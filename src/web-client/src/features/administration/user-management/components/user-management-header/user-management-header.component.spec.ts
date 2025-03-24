import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementHeaderComponent } from './user-management-header.component';

describe('UserManagementHeaderComponent', () => {
  let component: UserManagementHeaderComponent;
  let fixture: ComponentFixture<UserManagementHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
