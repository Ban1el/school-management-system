import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs';
import { UserAuthService } from '../services/user-auth-service';
import { ToastService } from '../services/toast-service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastServices = inject(ToastService);
  const userService = inject(UserAuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error) => {
      switch (error.status) {
        case 400:
          throw error;
        case 401:
          userService.currentUser.set(null);
          userService.logout();
          router.navigateByUrl('/');
          break;
        case 404:
          router.navigateByUrl('/not-found');
          break;
        case 500:
          const navigationExtras: NavigationExtras = { state: { error: error.error } };
          router.navigateByUrl('/server-error', navigationExtras);
          break;
        default:
          toastServices.error('Something went wrong');
          break;
      }
      throw error;
    }),
  );
};
