import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ColumnDefDirective } from '../../components/data-table-2/column-def.directive';
import { DataTableComponent, TableColumn } from '../../components/data-table-2/data-table.component';

@Component({
  selector: 'nc-data-table-page',
  standalone: true,
  imports: [CommonModule, DataTableComponent, ColumnDefDirective],
  templateUrl: './data-table-page.html',
  styleUrl: './data-table-page.css',
})
export class DataTablePage {
  columns: TableColumn[] = [
    { key: 'id', label: 'ID', sortable: true, width: '60px', align: 'center' },
    { key: 'name', label: 'Name', sortable: true },
    { key: 'email', label: 'Email', sortable: true },
    { key: 'role', label: 'Role', sortable: true },
    { key: 'status', label: 'Status', sortable: true },
    { key: 'actions', label: 'Actions', sortable: false, width: '140px', align: 'center' },
  ];

  users = [
    { id: 1, name: 'Alice Nguyen', email: 'alice@acme.io', role: 'Admin', status: 'Active' },
    { id: 2, name: 'Bruno Silva', email: 'bruno@acme.io', role: 'Engineer', status: 'Active' },
    { id: 3, name: 'Celine Dubois', email: 'celine@acme.io', role: 'Designer', status: 'Inactive' },
    { id: 4, name: 'Daniel Park', email: 'dpark@acme.io', role: 'Engineer', status: 'Active' },
    { id: 5, name: 'Elif Yilmaz', email: 'elif@acme.io', role: 'Manager', status: 'Active' },
    { id: 6, name: 'Felix Okoye', email: 'felix@acme.io', role: 'Engineer', status: 'Active' },
    { id: 7, name: 'Grace Kim', email: 'grace@acme.io', role: 'Designer', status: 'Inactive' },
  ];

  edit(row: any) { console.log('Edit', row); }
  remove(row: any) { this.users = this.users.filter((u) => u.id !== row.id); }
}
