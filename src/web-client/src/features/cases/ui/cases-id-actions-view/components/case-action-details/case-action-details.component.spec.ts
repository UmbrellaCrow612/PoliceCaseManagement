import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaseActionDetailsComponent } from './case-action-details.component';

describe('CaseActionDetailsComponent', () => {
  let component: CaseActionDetailsComponent;
  let fixture: ComponentFixture<CaseActionDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseActionDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaseActionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
