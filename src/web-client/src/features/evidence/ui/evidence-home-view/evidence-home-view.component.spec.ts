import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EvidenceHomeViewComponent } from './evidence-home-view.component';

describe('EvidenceHomeViewComponent', () => {
  let component: EvidenceHomeViewComponent;
  let fixture: ComponentFixture<EvidenceHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EvidenceHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EvidenceHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
