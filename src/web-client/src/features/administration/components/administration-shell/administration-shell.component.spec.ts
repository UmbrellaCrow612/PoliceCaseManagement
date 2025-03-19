import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationShellComponent } from './administration-shell.component';

describe('AdministrationShellComponent', () => {
  let component: AdministrationShellComponent;
  let fixture: ComponentFixture<AdministrationShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdministrationShellComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdministrationShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
