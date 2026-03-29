import { Routes } from '@angular/router';
import { Login } from '../features/authentication/login/login';
import { UserManagement } from '../features/user-management/user-management';

export const routes: Routes = [
  { path: '', component: Login },
  { path: 'user/management', component: UserManagement },
];
