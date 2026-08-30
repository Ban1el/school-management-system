import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { RoleDto } from '../../types/Role/RoleDto';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}role`;

  getRoles() {
    return this.http.get<RoleDto[]>(`${this.baseUrl}/all/active`);
  }
}
