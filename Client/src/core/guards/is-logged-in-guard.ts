import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserAuthService } from '../services/user-auth-service';

export const isLoggedInGuard: CanActivateFn = (route, state) => {
  const userAuthService = inject(UserAuthService);
  const router = inject(Router);

  if (userAuthService.isAuthenticated()) {
    router.navigateByUrl('/home');
    return false;
  }

  return true;
};
