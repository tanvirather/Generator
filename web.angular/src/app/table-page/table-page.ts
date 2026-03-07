import { Component } from '@angular/core';
import { Components } from '../../components';
import { TableColumn } from '../../components/table/tableColumn';

@Component({
  selector: 'app-table-page',
  imports: [...Components],
  templateUrl: './table-page.html',
  styleUrl: './table-page.css',
})
export class TablePage {
  columns: TableColumn[] = [
    { key: 'id', label: 'ID', sortable: true, width: '60px' },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'email', label: 'Email', sortable: true },
    { key: 'role', label: 'Role', sortable: true },
    { key: 'status', label: 'Status', sortable: true },
  ];

  dataList = [
    { id: 1, name: 'Alice Nguyen', email: 'alice@acme.io', role: 'Admin', status: 'Active' },
    { id: 2, name: 'Bruno Silva', email: 'bruno@acme.io', role: 'Engineer', status: 'Active' },
    { id: 3, name: 'Celine Dubois', email: 'celine@acme.io', role: 'Designer', status: 'Inactive' },
    { id: 4, name: 'Daniel Park', email: 'dpark@acme.io', role: 'Engineer', status: 'Active' },
    { id: 5, name: 'Elif Yilmaz', email: 'elif@acme.io', role: 'Manager', status: 'Active' },
    { id: 6, name: 'Felix Okoye', email: 'felix@acme.io', role: 'Engineer', status: 'Active' },
    { id: 7, name: 'Grace Kim', email: 'grace@acme.io', role: 'Designer', status: 'Inactive' },
  ]
}
