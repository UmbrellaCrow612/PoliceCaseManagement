import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoFactorSmsViewComponent } from './two-factor-sms-view.component';

describe('TwoFactorSmsViewComponent', () => {
  let component: TwoFactorSmsViewComponent;
  let fixture: ComponentFixture<TwoFactorSmsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoFactorSmsViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoFactorSmsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
