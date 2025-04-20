import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesShellComponent } from './cases-shell.component';

describe('CasesShellComponent', () => {
  let component: CasesShellComponent;
  let fixture: ComponentFixture<CasesShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesShellComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
