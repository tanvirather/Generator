import { PostgresTypeStore } from './postgres-type-store';

describe('PostgresTypeStore', () => {
  it('should create an instance', () => {
    expect(new PostgresTypeStore()).toBeTruthy();
  });
});
