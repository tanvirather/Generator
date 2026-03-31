import { Routes } from '@angular/router';
import { DataTablePage } from './data-table-page/data-table-page';
import { TablePage } from './table-page/table-page';
import { PostgresType } from './postgres-type/postgres-type';
import {Index} from './index';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./login/login').then(m => m.Login) },
  {
    path: "",
    component: Index,
    // canActivate: [AuthGuard],
    children: [
      { path: "postgres-type", component: PostgresType },
      { path: 'table-page', component: TablePage },
      { path: 'data-table-page', component: DataTablePage },
    ]
  },
  { path: "**", redirectTo: "" },
  // { path: "**", redirectTo: "" },
];


// { path: 'table-page', component: TablePage },
// { path: 'data-table-page', component: DataTablePage },
// { path: "postgres-type", component: PostgresType },
// { path: '', redirectTo: 'login', pathMatch: 'full' },
// { path: 'table-page', loadComponent: () => import('./table-page/table-page').then(m => m.TablePage) },
// { path: '**', redirectTo: 'login' },


