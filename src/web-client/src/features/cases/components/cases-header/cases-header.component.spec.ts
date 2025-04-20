import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesHeaderComponent } from './cases-header.component';

describe('CasesHeaderComponent', () => {
  let component: CasesHeaderComponent;
  let fixture: ComponentFixture<CasesHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
