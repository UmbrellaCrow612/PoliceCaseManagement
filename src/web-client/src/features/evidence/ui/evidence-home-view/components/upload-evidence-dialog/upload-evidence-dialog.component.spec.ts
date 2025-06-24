import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadEvidenceDialogComponent } from './upload-evidence-dialog.component';

describe('UploadEvidenceDialogComponent', () => {
  let component: UploadEvidenceDialogComponent;
  let fixture: ComponentFixture<UploadEvidenceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadEvidenceDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadEvidenceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
