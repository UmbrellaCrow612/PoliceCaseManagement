import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeCreateComponent } from './system-incident-type-create.component';

describe('SystemIncidentTypeCreateComponent', () => {
  let component: SystemIncidentTypeCreateComponent;
  let fixture: ComponentFixture<SystemIncidentTypeCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
