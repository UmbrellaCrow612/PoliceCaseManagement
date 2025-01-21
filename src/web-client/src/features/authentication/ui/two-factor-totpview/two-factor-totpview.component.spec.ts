import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoFactorTOTPViewComponent } from './two-factor-totpview.component';

describe('TwoFactorTOTPViewComponent', () => {
  let component: TwoFactorTOTPViewComponent;
  let fixture: ComponentFixture<TwoFactorTOTPViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoFactorTOTPViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoFactorTOTPViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
