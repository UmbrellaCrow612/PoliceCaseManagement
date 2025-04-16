import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeIdViewComponent } from './system-incident-type-id-view.component';

describe('SystemIncidentTypeIdViewComponent', () => {
  let component: SystemIncidentTypeIdViewComponent;
  let fixture: ComponentFixture<SystemIncidentTypeIdViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeIdViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeIdViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
