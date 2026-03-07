import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TableColumn } from './tableColumn';

@Component({
  selector: 'nc-table',
  imports: [CommonModule],
  templateUrl: './table.html',
  styleUrl: './table.css',
})
export class Table {
  @Input() columns: TableColumn[] = [];
  @Input() dataList: any[] = [];
}
