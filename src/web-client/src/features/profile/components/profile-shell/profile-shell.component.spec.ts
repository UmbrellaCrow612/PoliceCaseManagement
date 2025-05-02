import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileShellComponent } from './profile-shell.component';

describe('ProfileShellComponent', () => {
  let component: ProfileShellComponent;
  let fixture: ComponentFixture<ProfileShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileShellComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
