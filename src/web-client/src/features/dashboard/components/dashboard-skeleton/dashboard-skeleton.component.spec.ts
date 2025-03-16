import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardSkeletonComponent } from './dashboard-skeleton.component';

describe('DashboardSkeletonComponent', () => {
  let component: DashboardSkeletonComponent;
  let fixture: ComponentFixture<DashboardSkeletonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardSkeletonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardSkeletonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
