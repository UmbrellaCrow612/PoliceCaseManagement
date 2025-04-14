import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeDisplayByIdComponent } from './system-incident-type-display-by-id.component';

describe('SystemIncidentTypeDisplayByIdComponent', () => {
  let component: SystemIncidentTypeDisplayByIdComponent;
  let fixture: ComponentFixture<SystemIncidentTypeDisplayByIdComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeDisplayByIdComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeDisplayByIdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
