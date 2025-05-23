import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignUserDialogComponent } from './assign-user-dialog.component';

describe('AssignUserDialogComponent', () => {
  let component: AssignUserDialogComponent;
  let fixture: ComponentFixture<AssignUserDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignUserDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssignUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
