import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordViewComponent } from './reset-password-view.component';

describe('ResetPasswordViewComponent', () => {
  let component: ResetPasswordViewComponent;
  let fixture: ComponentFixture<ResetPasswordViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResetPasswordViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResetPasswordViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
