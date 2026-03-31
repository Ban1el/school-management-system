import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { UserAuthService } from '../services/user-auth-service';

export const refreshTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const userAuthService = inject(UserAuthService);
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
            return throwError(() => error);
          }),
        );
      }

      return throwError(() => error);
    }),
  );
};
