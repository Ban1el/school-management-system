import { HttpClient, HttpContext } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { UserDto } from '../../types/User/UserDto';
import { environment } from '../../environments/environment.development';
import { UserSigninDto } from '../../types/User/UserSigninDto';
import { tap } from 'rxjs';
import { SkipLoading } from '../interceptors/loading-interceptor';

@Injectable({
  providedIn: 'root',
})
export class UserAuthService {
  private http = inject(HttpClient);
  currentUser = signal<UserDto | null>(null);
  private baseUrl = environment.apiUrl;

  login(dto: UserSigninDto) {
    return this.http
      .post<UserDto>(this.baseUrl + 'user/auth/signin', dto, {
        withCredentials: true,
      })
      .pipe(
        tap((user) => {
          if (user) {
            this.setCurrentUser(user);
          }
        }),
      );
  }

  setCurrentUser(user: UserDto) {
    this.currentUser.set(user);
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
      .pipe(
        tap(() => {
          this.currentUser.set(null);
        }),
      );
  }
}
