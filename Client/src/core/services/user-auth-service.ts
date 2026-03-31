import { HttpClient, HttpContext } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { UserDto } from '../../types/User/UserDto';
import { environment } from '../../environments/environment.development';
import { UserSigninDto } from '../../types/User/UserSigninDto';
import { catchError, map, Observable, of, tap } from 'rxjs';
import { SkipLoading } from '../interceptors/loading-interceptor';

@Injectable({
  providedIn: 'root',
})
export class UserAuthService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private _isAuthenticated = signal(false);
  isAuthenticated = computed(() => this._isAuthenticated());

  login(dto: UserSigninDto) {
    return this.http
      .post<UserDto>(this.baseUrl + 'user/auth/signin', dto, {
        withCredentials: true,
      })
      .pipe(tap(() => this._isAuthenticated.set(true)));
  }

  verifySession(): Observable<boolean> {
    return this.http.get(this.baseUrl + 'user/auth/verify').pipe(
      tap(() => this._isAuthenticated.set(true)),
      map(() => true),
      catchError(() => of(false)),
    );
  }

  setCurrentUser(user: UserDto) {
    //To be used later
    // this.currentUser.set(user);
  }

  logout() {
    return this.http
      .post(
        this.baseUrl + 'user/auth/logout',
        {},
        {
          withCredentials: true,
        },
      )
      .pipe(tap(() => this._isAuthenticated.set(false)));
  }

  refreshToken() {
    return this.http.post(this.baseUrl + 'user/auth/refresh/token', {});
  }
}
