import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesCreateViewComponent } from './cases-create-view.component';

describe('CasesCreateViewComponent', () => {
  let component: CasesCreateViewComponent;
  let fixture: ComponentFixture<CasesCreateViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesCreateViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesCreateViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
