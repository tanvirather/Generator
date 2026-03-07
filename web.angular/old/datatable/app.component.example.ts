// app.component.ts — Usage Example
import { Component } from '@angular/core';
import { DataTableComponent, TableColumn } from './data-table/data-table.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [DataTableComponent],
  template: `
    <div style="padding: 40px; background: #0f0f11; min-height: 100vh;">
      <app-data-table
        [columns]="columns"
        [data]="users"
        [pageSize]="5"
        [showSearch]="true"
        [showPagination]="true"
        caption="User Directory"
      />
    </div>
  `,
})
export class AppComponent {
  columns: TableColumn[] = [
    { key: 'id',     label: 'ID',         sortable: true,  width: '60px',  align: 'center' },
    { key: 'name',   label: 'Name',        sortable: true },
    { key: 'email',  label: 'Email',       sortable: true },
    { key: 'role',   label: 'Role',        sortable: true },
    { key: 'status', label: 'Status',      sortable: true },
    {
      key: 'joined',
      label: 'Joined',
      sortable: true,
      format: (val) => new Date(val).toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' }),
    },
  ];

  users = [
    { id: 1,  name: 'Alice Nguyen',    email: 'alice@acme.io',    role: 'Admin',     status: 'Active',   joined: '2022-03-15' },
    { id: 2,  name: 'Bruno Silva',     email: 'bruno@acme.io',    role: 'Engineer',  status: 'Active',   joined: '2023-01-08' },
    { id: 3,  name: 'Celine Dubois',   email: 'celine@acme.io',   role: 'Designer',  status: 'Inactive', joined: '2021-11-22' },
    { id: 4,  name: 'Daniel Park',     email: 'dpark@acme.io',    role: 'Engineer',  status: 'Active',   joined: '2023-06-01' },
    { id: 5,  name: 'Elif Yilmaz',     email: 'elif@acme.io',     role: 'Manager',   status: 'Active',   joined: '2020-09-17' },
    { id: 6,  name: 'Felix Okoye',     email: 'felix@acme.io',    role: 'Engineer',  status: 'Active',   joined: '2024-02-14' },
    { id: 7,  name: 'Grace Kim',       email: 'grace@acme.io',    role: 'Designer',  status: 'Active',   joined: '2022-07-30' },
    { id: 8,  name: 'Hugo Ferreira',   email: 'hugo@acme.io',     role: 'Admin',     status: 'Inactive', joined: '2019-05-03' },
    { id: 9,  name: 'Ingrid Larsson',  email: 'ingrid@acme.io',   role: 'Engineer',  status: 'Active',   joined: '2023-10-11' },
    { id: 10, name: 'James Obi',       email: 'james@acme.io',    role: 'Manager',   status: 'Active',   joined: '2021-04-19' },
    { id: 11, name: 'Karla Reyes',     email: 'karla@acme.io',    role: 'Designer',  status: 'Active',   joined: '2024-01-07' },
    { id: 12, name: 'Luca Bianchi',    email: 'luca@acme.io',     role: 'Engineer',  status: 'Inactive', joined: '2022-12-25' },
  ];
}
