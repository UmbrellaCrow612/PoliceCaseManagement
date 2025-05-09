import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdActionsViewComponent } from './cases-id-actions-view.component';

describe('CasesIdActionsViewComponent', () => {
  let component: CasesIdActionsViewComponent;
  let fixture: ComponentFixture<CasesIdActionsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdActionsViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdActionsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
