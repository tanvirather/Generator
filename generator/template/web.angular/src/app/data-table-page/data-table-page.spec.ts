import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataTablePage } from './data-table-page';

describe('DataTablePage', () => {
  let component: DataTablePage;
  let fixture: ComponentFixture<DataTablePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataTablePage],
    }).compileComponents();

    fixture = TestBed.createComponent(DataTablePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
