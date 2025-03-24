import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementCreateViewComponent } from './user-management-create-view.component';

describe('UserManagementCreateViewComponent', () => {
  let component: UserManagementCreateViewComponent;
  let fixture: ComponentFixture<UserManagementCreateViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementCreateViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementCreateViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
