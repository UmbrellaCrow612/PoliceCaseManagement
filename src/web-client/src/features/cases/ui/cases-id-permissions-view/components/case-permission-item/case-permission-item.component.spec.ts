import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasePermissionItemComponent } from './case-permission-item.component';

describe('CasePermissionItemComponent', () => {
  let component: CasePermissionItemComponent;
  let fixture: ComponentFixture<CasePermissionItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasePermissionItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasePermissionItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
