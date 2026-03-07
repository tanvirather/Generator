// app.component.ts — Usage Example with Templated Columns
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTableComponent, TableColumn } from './data-table/data-table.component';
import { ColumnDefDirective } from './data-table/column-def.directive';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, DataTableComponent, ColumnDefDirective],
  template: `
    <div style="padding: 40px; background: #0f0f11; min-height: 100vh;">
      <app-data-table
        [columns]="columns"
        [data]="users"
        [pageSize]="5"
        caption="User Directory"
      >
        <!--
          Templated cell for "status" column.
          $implicit  = row['status']  (the raw cell value)
          row        = the full row object
        -->
        <ng-template columnDef="status" let-value let-row="row">
          <span class="badge" [class]="'badge--' + value.toLowerCase()">
            {{ value }}
          </span>
        </ng-template>

        <!--
          Templated cell for "name" column with avatar initials.
        -->
        <ng-template columnDef="name" let-value let-row="row">
          <div class="user-cell">
            <span class="avatar">{{ value[0] }}</span>
            <span>{{ value }}</span>
          </div>
        </ng-template>

        <!--
          Custom header template for the "actions" column.
          The $implicit context receives the TableColumn definition.
        -->
        <ng-template columnDef="actions" let-value let-row="row">
          <div class="actions">
            <button (click)="edit(row)">Edit</button>
            <button (click)="remove(row)">Delete</button>
          </div>
        </ng-template>
      </app-data-table>
    </div>
  `,
  styles: [`
    .badge { padding: 2px 10px; border-radius: 999px; font-size: 0.75rem; font-weight: 600; }
    .badge--active   { background: rgba(200,241,53,0.15); color: #c8f135; }
    .badge--inactive { background: rgba(255,80,80,0.12);  color: #ff6060; }

    .user-cell { display: flex; align-items: center; gap: 10px; }
    .avatar { width: 28px; height: 28px; border-radius: 50%; background: #2a2a32;
              display: grid; place-items: center; font-size: 0.8rem; font-weight: 700; color: #c8f135; }

    .actions { display: flex; gap: 8px; }
    .actions button { background: #2a2a32; border: none; color: #ccc; padding: 4px 10px;
                      border-radius: 4px; cursor: pointer; font-size: 0.75rem; }
    .actions button:hover { background: #3a3a42; color: #fff; }
  `],
})
export class AppComponent {
  columns: TableColumn[] = [
    { key: 'id',      label: 'ID',      sortable: true, width: '60px',  align: 'center' },
    { key: 'name',    label: 'Name',    sortable: true },
    { key: 'email',   label: 'Email',   sortable: true },
    { key: 'role',    label: 'Role',    sortable: true },
    { key: 'status',  label: 'Status',  sortable: true },
    { key: 'actions', label: 'Actions', sortable: false, width: '140px', align: 'center' },
  ];

  users = [
    { id: 1,  name: 'Alice Nguyen',   email: 'alice@acme.io',  role: 'Admin',    status: 'Active'   },
    { id: 2,  name: 'Bruno Silva',    email: 'bruno@acme.io',  role: 'Engineer', status: 'Active'   },
    { id: 3,  name: 'Celine Dubois',  email: 'celine@acme.io', role: 'Designer', status: 'Inactive' },
    { id: 4,  name: 'Daniel Park',    email: 'dpark@acme.io',  role: 'Engineer', status: 'Active'   },
    { id: 5,  name: 'Elif Yilmaz',    email: 'elif@acme.io',   role: 'Manager',  status: 'Active'   },
    { id: 6,  name: 'Felix Okoye',    email: 'felix@acme.io',  role: 'Engineer', status: 'Active'   },
    { id: 7,  name: 'Grace Kim',      email: 'grace@acme.io',  role: 'Designer', status: 'Inactive' },
  ];

  edit(row: any) { console.log('Edit', row); }
  remove(row: any) { this.users = this.users.filter((u) => u.id !== row.id); }
}
