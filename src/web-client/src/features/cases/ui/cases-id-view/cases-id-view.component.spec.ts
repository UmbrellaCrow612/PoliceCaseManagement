import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdViewComponent } from './cases-id-view.component';

describe('CasesIdViewComponent', () => {
  let component: CasesIdViewComponent;
  let fixture: ComponentFixture<CasesIdViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
