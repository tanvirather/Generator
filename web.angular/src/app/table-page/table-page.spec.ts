import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TablePage } from './table-page';

describe('TablePage', () => {
  let component: TablePage;
  let fixture: ComponentFixture<TablePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TablePage],
    }).compileComponents();

    fixture = TestBed.createComponent(TablePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
