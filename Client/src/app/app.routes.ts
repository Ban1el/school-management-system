import { Routes } from '@angular/router';
import { Login } from '../features/authentication/login/login';
import { UserManagement } from '../features/user-management/user-management';
import { Home } from '../features/home/home';
import { Nav } from '../layout/nav/nav';
import { authGuard } from '../core/guards/auth-guard';
import { isLoggedInGuard } from '../core/guards/is-logged-in-guard';

export const routes: Routes = [
  { path: '', component: Login, canActivate: [isLoggedInGuard] },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    component: Nav,
    children: [
      { path: 'home', component: Home },
      { path: 'user/management', component: UserManagement },
    ],
  },
];
