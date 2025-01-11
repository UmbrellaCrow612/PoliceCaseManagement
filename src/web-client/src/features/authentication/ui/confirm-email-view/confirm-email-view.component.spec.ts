import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailViewComponent } from './confirm-email-view.component';

describe('ConfirmEmailViewComponent', () => {
  let component: ConfirmEmailViewComponent;
  let fixture: ComponentFixture<ConfirmEmailViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmEmailViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmEmailViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
