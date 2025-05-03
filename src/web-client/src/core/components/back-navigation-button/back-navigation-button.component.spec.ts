import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackNavigationButtonComponent } from './back-navigation-button.component';

describe('BackNavigationButtonComponent', () => {
  let component: BackNavigationButtonComponent;
  let fixture: ComponentFixture<BackNavigationButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BackNavigationButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BackNavigationButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
