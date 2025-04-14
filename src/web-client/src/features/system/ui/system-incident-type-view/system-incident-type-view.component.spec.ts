import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeViewComponent } from './system-incident-type-view.component';

describe('SystemIncidentTypeViewComponent', () => {
  let component: SystemIncidentTypeViewComponent;
  let fixture: ComponentFixture<SystemIncidentTypeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
