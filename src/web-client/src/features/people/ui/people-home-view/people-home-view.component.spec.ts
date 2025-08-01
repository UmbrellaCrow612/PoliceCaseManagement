import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PeopleHomeViewComponent } from './people-home-view.component';

describe('PeopleHomeViewComponent', () => {
  let component: PeopleHomeViewComponent;
  let fixture: ComponentFixture<PeopleHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PeopleHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PeopleHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
