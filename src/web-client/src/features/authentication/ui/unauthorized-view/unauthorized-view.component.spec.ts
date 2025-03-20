import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnauthorizedViewComponent } from './unauthorized-view.component';

describe('UnauthorizedViewComponent', () => {
  let component: UnauthorizedViewComponent;
  let fixture: ComponentFixture<UnauthorizedViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UnauthorizedViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnauthorizedViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
