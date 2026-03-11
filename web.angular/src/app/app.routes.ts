import { Routes } from '@angular/router';
import { TablePage } from './table-page/table-page';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./login/login').then(m => m.Login) },
  { path: 'table-page', component: TablePage },
  // { path: 'table-page', loadComponent: () => import('./table-page/table-page').then(m => m.TablePage) },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  // { path: '**', redirectTo: 'login' },
];


