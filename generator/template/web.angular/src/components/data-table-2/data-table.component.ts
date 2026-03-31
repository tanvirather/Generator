import { CommonModule } from '@angular/common';
import {
  AfterContentInit,
  Component,
  ContentChildren,
  Input,
  OnChanges,
  OnInit,
  QueryList,
  SimpleChanges,
  TemplateRef,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ColumnDefDirective } from './column-def.directive';

export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  width?: string;
  align?: 'left' | 'center' | 'right';
  /** Fallback string formatter — ignored when a matching columnDef template exists */
  format?: (value: any, row: any) => string;
}

export type SortDirection = 'asc' | 'desc' | null;

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.scss'],
})
export class DataTableComponent implements OnInit, OnChanges, AfterContentInit {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() pageSize: number = 10;
  @Input() showSearch: boolean = true;
  @Input() showPagination: boolean = true;
  @Input() caption: string = '';

  /** Collect all <ng-template columnDef="..."> children */
  @ContentChildren(ColumnDefDirective)
  private columnDefs!: QueryList<ColumnDefDirective>;

  /** Resolved map of key → cell TemplateRef */
  cellTemplates = new Map<string, TemplateRef<any>>();
  /** Resolved map of key → header TemplateRef */
  headerTemplates = new Map<string, TemplateRef<any>>();

  searchQuery: string = '';
  currentPage: number = 1;
  sortKey: string | null = null;
  sortDirection: SortDirection = null;

  filteredData: any[] = [];
  pagedData: any[] = [];
  totalPages: number = 1;

  ngOnInit(): void {
    this.process();
  }

  ngAfterContentInit(): void {
    this.syncTemplates();
    this.columnDefs.changes.subscribe(() => this.syncTemplates());
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] || changes['columns']) {
      this.process();
    }
  }

  private syncTemplates(): void {
    this.cellTemplates.clear();
    this.headerTemplates.clear();
    this.columnDefs.forEach((def) => {
      this.cellTemplates.set(def.key, def.tpl);
      if (def.headerTpl) {
        this.headerTemplates.set(def.key, def.headerTpl);
      }
    });
  }

  process(): void {
    let result = [...this.data];

    if (this.searchQuery.trim()) {
      const q = this.searchQuery.toLowerCase();
      result = result.filter((row) =>
        this.columns.some((col) =>
          String(row[col.key] ?? '').toLowerCase().includes(q)
        )
      );
    }

    if (this.sortKey && this.sortDirection) {
      const dir = this.sortDirection === 'asc' ? 1 : -1;
      const key = this.sortKey;
      result = result.sort((a, b) => {
        const av = a[key] ?? '';
        const bv = b[key] ?? '';
        if (av < bv) return -1 * dir;
        if (av > bv) return 1 * dir;
        return 0;
      });
    }

    this.filteredData = result;
    this.totalPages = Math.max(1, Math.ceil(result.length / this.pageSize));
    if (this.currentPage > this.totalPages) this.currentPage = 1;
    this.updatePage();
  }

  updatePage(): void {
    const start = (this.currentPage - 1) * this.pageSize;
    this.pagedData = this.filteredData.slice(start, start + this.pageSize);
  }

  onSearch(): void {
    this.currentPage = 1;
    this.process();
  }

  sort(col: TableColumn): void {
    if (!col.sortable) return;
    if (this.sortKey === col.key) {
      this.sortDirection =
        this.sortDirection === 'asc'
          ? 'desc'
          : this.sortDirection === 'desc'
            ? null
            : 'asc';
      if (!this.sortDirection) this.sortKey = null;
    } else {
      this.sortKey = col.key;
      this.sortDirection = 'asc';
    }
    this.process();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePage();
  }

  getCellValue(row: any, col: TableColumn): string {
    const raw = row[col.key];
    return col.format ? col.format(raw, row) : String(raw ?? '—');
  }

  getCellContext(row: any, col: TableColumn): { $implicit: any; row: any } {
    return { $implicit: row[col.key], row };
  }

  getSortIcon(col: TableColumn): string {
    if (!col.sortable) return '';
    if (this.sortKey !== col.key) return '↕';
    return this.sortDirection === 'asc' ? '↑' : '↓';
  }

  get pageNumbers(): number[] {
    const total = this.totalPages;
    const current = this.currentPage;
    const delta = 2;
    const range: number[] = [];
    for (
      let i = Math.max(1, current - delta);
      i <= Math.min(total, current + delta);
      i++
    ) {
      range.push(i);
    }
    return range;
  }

  get startIndex(): number {
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get endIndex(): number {
    return Math.min(this.currentPage * this.pageSize, this.filteredData.length);
  }
}
