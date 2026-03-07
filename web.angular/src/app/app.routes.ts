import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'table-page', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./login/login').then(m => m.Login) },
  { path: 'table-page', loadComponent: () => import('./table-page/table-page').then(m => m.TablePage) },
  { path: '**', redirectTo: 'login' },
];
