import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostgresType } from './postgres-type';

describe('PostgresType', () => {
  let component: PostgresType;
  let fixture: ComponentFixture<PostgresType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostgresType],
    }).compileComponents();

    fixture = TestBed.createComponent(PostgresType);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
