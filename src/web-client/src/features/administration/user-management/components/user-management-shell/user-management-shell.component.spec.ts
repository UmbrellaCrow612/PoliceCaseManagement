import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementShellComponent } from './user-management-shell.component';

describe('UserManagementShellComponent', () => {
  let component: UserManagementShellComponent;
  let fixture: ComponentFixture<UserManagementShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserManagementShellComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserManagementShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
