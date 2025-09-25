import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppSettingsDriftViewComponent } from './app-settings-drift-view.component';

describe('AppSettingsDriftViewComponent', () => {
  let component: AppSettingsDriftViewComponent;
  let fixture: ComponentFixture<AppSettingsDriftViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppSettingsDriftViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppSettingsDriftViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
