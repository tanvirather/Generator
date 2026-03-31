import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Components } from '../../components';
import { TableColumn } from '../../components/table/tableColumn';
import { TableStore } from '../../store';

@Component({
  selector: 'app-table-page',
  imports: [CommonModule, ...Components],
  templateUrl: './table-page.html',
  styleUrl: './table-page.css',
  standalone: true,
})
export class TablePage {

  public tableStore = new TableStore();

  columns: TableColumn[] = [
    { key: 'id', label: 'ID' },
    { key: 'name', label: 'Name' },
    { key: 'email', label: 'Email' },
    { key: 'role', label: 'Role' },
    { key: 'status', label: 'Status' },
  ];
}
