import { PostgresType } from '../models';
import { BaseStore } from './baseStore';

export class PostgresTypeStore extends BaseStore<PostgresType> {
  constructor() {
    super('http://localhost:5051/PostgresType');
    // super('product/PostgresType');
  }
}
