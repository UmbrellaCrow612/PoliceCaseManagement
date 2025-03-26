import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementUserIdViewComponent } from './user-management-user-id-view.component';

describe('UserManagementUserIdViewComponent', () => {
  let component: UserManagementUserIdViewComponent;
  let fixture: ComponentFixture<UserManagementUserIdViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementUserIdViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementUserIdViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
