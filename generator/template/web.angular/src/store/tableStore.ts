import {BaseStore} from './baseStore';

export class TableStore extends BaseStore {
  constructor() {
    super('/api/table');
  }

  override async get(): Promise<any> {
    return [
      { id: 1, name: 'Alice Nguyen', email: 'alice@acme.io', role: 'Admin', status: 'Active' },
      { id: 2, name: 'Bruno Silva', email: 'bruno@acme.io', role: 'Engineer', status: 'Active' },
      { id: 3, name: 'Celine Dubois', email: 'celine@acme.io', role: 'Designer', status: 'Inactive' },
      { id: 4, name: 'Daniel Park', email: 'dpark@acme.io', role: 'Engineer', status: 'Active' },
      { id: 5, name: 'Elif Yilmaz', email: 'elif@acme.io', role: 'Manager', status: 'Active' },
      { id: 6, name: 'Felix Okoye', email: 'felix@acme.io', role: 'Engineer', status: 'Active' },
      { id: 7, name: 'Grace Kim', email: 'grace@acme.io', role: 'Designer', status: 'Inactive' },
    ];
  }
}
