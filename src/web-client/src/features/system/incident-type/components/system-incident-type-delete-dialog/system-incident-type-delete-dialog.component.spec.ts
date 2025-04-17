import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemIncidentTypeDeleteDialogComponent } from './system-incident-type-delete-dialog.component';

describe('SystemIncidentTypeDeleteDialogComponent', () => {
  let component: SystemIncidentTypeDeleteDialogComponent;
  let fixture: ComponentFixture<SystemIncidentTypeDeleteDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemIncidentTypeDeleteDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemIncidentTypeDeleteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
