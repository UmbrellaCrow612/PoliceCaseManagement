import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveAssignedUserDialogComponent } from './remove-assigned-user-dialog.component';

describe('RemoveAssignedUserDialogComponent', () => {
  let component: RemoveAssignedUserDialogComponent;
  let fixture: ComponentFixture<RemoveAssignedUserDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveAssignedUserDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemoveAssignedUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
