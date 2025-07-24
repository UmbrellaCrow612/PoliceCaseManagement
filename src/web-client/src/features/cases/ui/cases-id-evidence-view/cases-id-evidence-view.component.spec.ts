import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdEvidenceViewComponent } from './cases-id-evidence-view.component';

describe('CasesIdEvidenceViewComponent', () => {
  let component: CasesIdEvidenceViewComponent;
  let fixture: ComponentFixture<CasesIdEvidenceViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdEvidenceViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdEvidenceViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
