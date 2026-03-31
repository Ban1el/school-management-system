import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from '../core/interceptors/error-interceptor';
import { jwtInterceptor } from '../core/interceptors/jwt-interceptor';
import { loadingInterceptor } from '../core/interceptors/loading-interceptor';
import { UserAuthService } from '../core/services/user-auth-service';
import { refreshTokenInterceptor } from '../core/interceptors/refresh-token-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(
      withInterceptors([
        errorInterceptor,
        jwtInterceptor,
        loadingInterceptor,
        refreshTokenInterceptor,
      ]),
    ),
    provideRouter(routes),
    provideAppInitializer(() => {
      const authService = inject(UserAuthService);
      return authService.verifySession();
    }),
  ],
};
