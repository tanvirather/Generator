import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Components } from '../../components';
import { TableColumn } from '../../components/table/tableColumn';

@Component({
  selector: 'app-table-page',
  imports: [CommonModule, ...Components],
  templateUrl: './table-page.html',
  styleUrl: './table-page.css',
  standalone: true,
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
    { id: 1, name: 'Alice Nguyen', email: '<a href="mailto:alice@acme.io">alice@acme.io</a>', role: 'Admin', status: 'Active' },
    { id: 2, name: 'Bruno Silva', email: '<a href="mailto:bruno@acme.io">bruno@acme.io</a>', role: 'Engineer', status: 'Active' },
    { id: 3, name: 'Celine Dubois', email: '<a href="mailto:celine@acme.io">celine@acme.io</a>', role: 'Designer', status: 'Inactive' },
    { id: 4, name: 'Daniel Park', email: '<a href="mailto:dpark@acme.io">dpark@acme.io</a>', role: 'Engineer', status: 'Active' },
    { id: 5, name: 'Elif Yilmaz', email: '<a href="mailto:elif@acme.io">elif@acme.io</a>', role: 'Manager', status: 'Active' },
    { id: 6, name: 'Felix Okoye', email: '<a href="mailto:felix@acme.io">felix@acme.io</a>', role: 'Engineer', status: 'Active' },
    { id: 7, name: 'Grace Kim', email: '<a href="mailto:grace@acme.io">grace@acme.io</a>', role: 'Designer', status: 'Inactive' },
  ]
}
