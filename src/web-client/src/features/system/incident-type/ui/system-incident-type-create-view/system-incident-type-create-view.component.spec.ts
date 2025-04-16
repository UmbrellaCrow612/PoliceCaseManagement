import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeCreateViewComponent } from './system-incident-type-create-view.component';

describe('SystemIncidentTypeCreateViewComponent', () => {
  let component: SystemIncidentTypeCreateViewComponent;
  let fixture: ComponentFixture<SystemIncidentTypeCreateViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeCreateViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeCreateViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
