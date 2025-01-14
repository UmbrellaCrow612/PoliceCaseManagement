import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDeviceChallengeViewComponent } from './confirm-device-challenge-view.component';

describe('ConfirmDeviceChallengeViewComponent', () => {
  let component: ConfirmDeviceChallengeViewComponent;
  let fixture: ComponentFixture<ConfirmDeviceChallengeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmDeviceChallengeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmDeviceChallengeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
