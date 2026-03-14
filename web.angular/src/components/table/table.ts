import { CommonModule } from '@angular/common';
import { Component, ContentChildren, Input, QueryList, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ColTemplate } from './col-template';
import { TableColumn } from './tableColumn';
@Component({
  selector: 'nc-table',
  imports: [CommonModule, FormsModule],
  templateUrl: './table.html',
  styleUrl: './table.css',
  standalone: true,
})
export class Table {
  @Input() columns: TableColumn[] = [];
  @Input() dataList: any[] = [];
  visibleDataList: any[] = [];

  @ContentChildren(ColTemplate) tableTemplates!: QueryList<ColTemplate>;
  cellTemplates = new Map<string, TemplateRef<any>>();
  searchQuery: string = '';

  ngAfterContentInit(): void {
    this.cellTemplates.clear();
    this.tableTemplates.forEach(item => this.cellTemplates.set(item.key, item.tpl));
    this.visibleDataList = this.dataList;
  }

  onSearch(): void {
    // Implement search logic here, e.g., filter dataList based on searchQuery
    this.visibleDataList = this.dataList.filter(item => Object.values(item).some(value =>
      (value as string).toString().toLowerCase().includes(this.searchQuery.toLowerCase())
    ));
  }
}
