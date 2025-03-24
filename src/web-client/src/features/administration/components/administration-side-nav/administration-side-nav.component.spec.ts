import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationSideNavComponent } from './administration-side-nav.component';

describe('AdministrationSideNavComponent', () => {
  let component: AdministrationSideNavComponent;
  let fixture: ComponentFixture<AdministrationSideNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdministrationSideNavComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdministrationSideNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
