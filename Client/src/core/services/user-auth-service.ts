import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { UserDto } from '../../types/User/UserDto';
import { environment } from '../../environments/environment.development';
import { UserSigninDto } from '../../types/User/UserSigninDto';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserAuthService {
  private http = inject(HttpClient);
  currentUser = signal<UserDto | null>(null);
  private baseUrl = environment.apiUrl;

  login(dto: UserSigninDto) {
    return this.http
      .post<UserDto>(this.baseUrl + 'users/auth/signin', dto, {
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
}
