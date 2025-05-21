import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaseActionDialogDetailsComponent } from './case-action-dialog-details.component';

describe('CaseActionDialogDetailsComponent', () => {
  let component: CaseActionDialogDetailsComponent;
  let fixture: ComponentFixture<CaseActionDialogDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseActionDialogDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaseActionDialogDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
