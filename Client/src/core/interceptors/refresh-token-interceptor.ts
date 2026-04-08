import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, switchMap, throwError } from 'rxjs';
import { UserAuthService } from '../services/user-auth-service';
import { ToastService } from '../services/toast-service';

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const userAuthService = inject(UserAuthService);
  const toastService = inject(ToastService);
  const router = inject(Router);

  if (req.url.includes('refresh/token') || req.url.includes('signin')) {
    return next(req);
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return userAuthService.refreshToken().pipe(
          switchMap(() => next(req)),
          catchError(() => {
            userAuthService.logout();
            router.navigateByUrl('/');
            toastService.error('Session expired. Please sign in again.');
            return EMPTY;
          }),
        );
      }

      return throwError(() => error);
    }),
  );
};
