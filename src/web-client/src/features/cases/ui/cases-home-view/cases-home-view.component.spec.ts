import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesHomeViewComponent } from './cases-home-view.component';

describe('CasesHomeViewComponent', () => {
  let component: CasesHomeViewComponent;
  let fixture: ComponentFixture<CasesHomeViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesHomeViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesHomeViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
