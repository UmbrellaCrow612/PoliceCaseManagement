import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeDisplayAllComponent } from './system-incident-type-display-all.component';

describe('SystemIncidentTypeDisplayAllComponent', () => {
  let component: SystemIncidentTypeDisplayAllComponent;
  let fixture: ComponentFixture<SystemIncidentTypeDisplayAllComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeDisplayAllComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeDisplayAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
