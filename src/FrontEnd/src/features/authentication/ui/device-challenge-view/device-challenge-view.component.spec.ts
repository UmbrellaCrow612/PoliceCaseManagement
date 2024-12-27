import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceChallengeViewComponent } from './device-challenge-view.component';

describe('DeviceChallengeViewComponent', () => {
  let component: DeviceChallengeViewComponent;
  let fixture: ComponentFixture<DeviceChallengeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeviceChallengeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceChallengeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
