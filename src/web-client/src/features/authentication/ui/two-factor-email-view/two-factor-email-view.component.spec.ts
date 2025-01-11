import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoFactorEmailViewComponent } from './two-factor-email-view.component';

describe('TwoFactorEmailViewComponent', () => {
  let component: TwoFactorEmailViewComponent;
  let fixture: ComponentFixture<TwoFactorEmailViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoFactorEmailViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoFactorEmailViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
