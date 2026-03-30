import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserAuthService } from '../services/user-auth-service';
import { ToastService } from '../services/toast-service';

export const authGuard: CanActivateFn = () => {
  const userAuthService = inject(UserAuthService);
  const toastService = inject(ToastService);
  const router = inject(Router);
  const user = userAuthService.currentUser();
  const hasRefreshToken = document.cookie.includes('refreshToken');

  if (hasRefreshToken == null) {
    toastService.error('Access Restricted');
    router.navigateByUrl('/login');
    return false;
  }

  return true;
};
