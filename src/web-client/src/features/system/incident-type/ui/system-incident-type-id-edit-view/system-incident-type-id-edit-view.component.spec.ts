import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeIdEditViewComponent } from './system-incident-type-id-edit-view.component';

describe('SystemIncidentTypeIdEditViewComponent', () => {
  let component: SystemIncidentTypeIdEditViewComponent;
  let fixture: ComponentFixture<SystemIncidentTypeIdEditViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeIdEditViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeIdEditViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
