import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesMeViewComponent } from './cases-me-view.component';

describe('CasesMeViewComponent', () => {
  let component: CasesMeViewComponent;
  let fixture: ComponentFixture<CasesMeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesMeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesMeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
