import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoFactorViewComponent } from './two-factor-view.component';

describe('TwoFactorViewComponent', () => {
  let component: TwoFactorViewComponent;
  let fixture: ComponentFixture<TwoFactorViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TwoFactorViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoFactorViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
