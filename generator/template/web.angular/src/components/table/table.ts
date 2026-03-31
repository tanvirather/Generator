import { CommonModule } from '@angular/common';
import { Component, ContentChildren, Input, QueryList, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BaseStore } from '../../store';
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
  @Input() store!: BaseStore;
  @ContentChildren(ColTemplate) tableTemplates!: QueryList<ColTemplate>;
  cellTemplates = new Map<string, TemplateRef<any>>();
  searchQuery: string = '';
  visibleDataList: any[] = [];
  private dataList: any[] = [];

  async ngAfterContentInit(): Promise<void> {
    this.cellTemplates.clear();
    this.tableTemplates.forEach(item => this.cellTemplates.set(item.key, item.tpl));
    if (this.store) {
      this.dataList = await this.store.get() || [];
    }
    this.visibleDataList = this.dataList;
  }

  onSearch(): void {
    // Implement search logic here, e.g., filter dataList based on searchQuery
    this.visibleDataList = this.dataList.filter(item => Object.values(item).some(value =>
      (value as string).toString().toLowerCase().includes(this.searchQuery.toLowerCase())
    ));
  }
}
