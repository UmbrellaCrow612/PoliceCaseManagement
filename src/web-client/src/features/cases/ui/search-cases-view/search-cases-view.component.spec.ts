import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchCasesViewComponent } from './search-cases-view.component';

describe('SearchCasesViewComponent', () => {
  let component: SearchCasesViewComponent;
  let fixture: ComponentFixture<SearchCasesViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchCasesViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchCasesViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
