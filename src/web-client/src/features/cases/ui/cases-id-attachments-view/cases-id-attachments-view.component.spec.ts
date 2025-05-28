import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdAttachmentsViewComponent } from './cases-id-attachments-view.component';

describe('CasesIdAttachmentsViewComponent', () => {
  let component: CasesIdAttachmentsViewComponent;
  let fixture: ComponentFixture<CasesIdAttachmentsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdAttachmentsViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdAttachmentsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
