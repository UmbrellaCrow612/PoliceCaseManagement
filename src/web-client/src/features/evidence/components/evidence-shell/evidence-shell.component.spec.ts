import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EvidenceShellComponent } from './evidence-shell.component';

describe('EvidenceShellComponent', () => {
  let component: EvidenceShellComponent;
  let fixture: ComponentFixture<EvidenceShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EvidenceShellComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EvidenceShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
