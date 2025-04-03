import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangePasswordViewComponent } from './change-password-view.component';

describe('ChangePasswordViewComponent', () => {
  let component: ChangePasswordViewComponent;
  let fixture: ComponentFixture<ChangePasswordViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangePasswordViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangePasswordViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
