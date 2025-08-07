import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPersonToCaseDialogComponent } from './add-person-to-case-dialog.component';

describe('AddPersonToCaseDialogComponent', () => {
  let component: AddPersonToCaseDialogComponent;
  let fixture: ComponentFixture<AddPersonToCaseDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddPersonToCaseDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddPersonToCaseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
