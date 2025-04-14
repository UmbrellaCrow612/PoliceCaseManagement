import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemHomeViewComponent } from './system-home-view.component';

describe('SystemHomeViewComponent', () => {
  let component: SystemHomeViewComponent;
  let fixture: ComponentFixture<SystemHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
