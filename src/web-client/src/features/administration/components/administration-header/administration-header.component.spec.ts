import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationHeaderComponent } from './administration-header.component';

describe('AdministrationHeaderComponent', () => {
  let component: AdministrationHeaderComponent;
  let fixture: ComponentFixture<AdministrationHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdministrationHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdministrationHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
