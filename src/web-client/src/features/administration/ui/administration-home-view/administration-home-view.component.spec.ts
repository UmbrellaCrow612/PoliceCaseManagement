import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationHomeViewComponent } from './administration-home-view.component';

describe('AdministrationHomeViewComponent', () => {
  let component: AdministrationHomeViewComponent;
  let fixture: ComponentFixture<AdministrationHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdministrationHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdministrationHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
