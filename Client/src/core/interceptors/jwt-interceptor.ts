import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { UserAuthService } from '../services/user-auth-service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const userAuthService = inject(UserAuthService);

  const user = userAuthService.currentUser();

  if (user) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${user.token}`,
      },
    });
  }

  return next(req);
};
