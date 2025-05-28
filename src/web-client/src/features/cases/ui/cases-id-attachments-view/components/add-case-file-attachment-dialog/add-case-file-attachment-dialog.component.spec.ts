import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCaseFileAttachmentDialogComponent } from './add-case-file-attachment-dialog.component';

describe('AddCaseFileAttachmentDialogComponent', () => {
  let component: AddCaseFileAttachmentDialogComponent;
  let fixture: ComponentFixture<AddCaseFileAttachmentDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCaseFileAttachmentDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCaseFileAttachmentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
