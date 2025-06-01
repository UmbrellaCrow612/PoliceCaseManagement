import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasesIdPermissionsViewComponent } from './cases-id-permissions-view.component';

describe('CasesIdPermissionsViewComponent', () => {
  let component: CasesIdPermissionsViewComponent;
  let fixture: ComponentFixture<CasesIdPermissionsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasesIdPermissionsViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasesIdPermissionsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
