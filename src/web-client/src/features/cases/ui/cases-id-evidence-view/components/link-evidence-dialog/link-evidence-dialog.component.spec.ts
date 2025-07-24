import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkEvidenceDialogComponent } from './link-evidence-dialog.component';

describe('LinkEvidenceDialogComponent', () => {
  let component: LinkEvidenceDialogComponent;
  let fixture: ComponentFixture<LinkEvidenceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LinkEvidenceDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LinkEvidenceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
