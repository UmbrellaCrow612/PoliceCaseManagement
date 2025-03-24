import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementHomeViewComponent } from './user-management-home-view.component';

describe('UserManagementHomeViewComponent', () => {
  let component: UserManagementHomeViewComponent;
  let fixture: ComponentFixture<UserManagementHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
