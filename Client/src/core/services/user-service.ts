import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { UserDto } from '../../types/User/UserDto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getUser(id: number) {
    return this.http.get<UserDto>(this.baseUrl + 'user/' + id);
  }
}
