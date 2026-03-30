import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserAuthService } from '../services/user-auth-service';

export const isLoggedInGuard: CanActivateFn = (route, state) => {
  const userAuthService = inject(UserAuthService);
  const router = inject(Router);
  const hasRefreshToken = document.cookie.includes('refreshToken');

  if (hasRefreshToken != null) {
    router.navigateByUrl('/home');
    return false;
  }

  return true;
};
