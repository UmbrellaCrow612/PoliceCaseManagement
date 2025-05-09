import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCaseActionDialogComponent } from './create-case-action-dialog.component';

describe('CreateCaseActionDialogComponent', () => {
  let component: CreateCaseActionDialogComponent;
  let fixture: ComponentFixture<CreateCaseActionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateCaseActionDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCaseActionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
