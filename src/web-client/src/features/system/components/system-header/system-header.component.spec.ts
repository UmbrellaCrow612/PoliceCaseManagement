import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemHeaderComponent } from './system-header.component';

describe('SystemHeaderComponent', () => {
  let component: SystemHeaderComponent;
  let fixture: ComponentFixture<SystemHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
