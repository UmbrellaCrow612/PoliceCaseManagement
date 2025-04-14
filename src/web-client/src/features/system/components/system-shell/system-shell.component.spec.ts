import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemShellComponent } from './system-shell.component';

describe('SystemShellComponent', () => {
  let component: SystemShellComponent;
  let fixture: ComponentFixture<SystemShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemShellComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
