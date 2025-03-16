import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardOverviewViewComponent } from './dashboard-overview-view.component';

describe('DashboardOverviewViewComponent', () => {
  let component: DashboardOverviewViewComponent;
  let fixture: ComponentFixture<DashboardOverviewViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardOverviewViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardOverviewViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
