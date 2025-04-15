import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeHomeViewComponent } from './system-incident-type-home-view.component';

describe('SystemIncidentTypeHomeViewComponent', () => {
  let component: SystemIncidentTypeHomeViewComponent;
  let fixture: ComponentFixture<SystemIncidentTypeHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
