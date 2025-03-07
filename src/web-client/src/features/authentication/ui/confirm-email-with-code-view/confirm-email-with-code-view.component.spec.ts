import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailWithCodeViewComponent } from './confirm-email-with-code-view.component';

describe('ConfirmEmailWithCodeViewComponent', () => {
  let component: ConfirmEmailWithCodeViewComponent;
  let fixture: ComponentFixture<ConfirmEmailWithCodeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmEmailWithCodeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmEmailWithCodeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
