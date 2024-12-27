import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhoneConfirmationViewComponent } from './phone-confirmation-view.component';

describe('PhoneConfirmationViewComponent', () => {
  let component: PhoneConfirmationViewComponent;
  let fixture: ComponentFixture<PhoneConfirmationViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PhoneConfirmationViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhoneConfirmationViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
