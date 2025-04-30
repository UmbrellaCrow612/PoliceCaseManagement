import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdEditIncidentTypeViewComponent } from './cases-id-edit-incident-type-view.component';

describe('CasesIdEditIncidentTypeViewComponent', () => {
  let component: CasesIdEditIncidentTypeViewComponent;
  let fixture: ComponentFixture<CasesIdEditIncidentTypeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdEditIncidentTypeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdEditIncidentTypeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
