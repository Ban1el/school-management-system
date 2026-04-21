import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { GenderDto } from '../../types/Gender/GenderDto';

@Injectable({
  providedIn: 'root',
})
export class GenderService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}gender`;

  getGendersActive() {
    return this.http.get<GenderDto[]>(`${this.baseUrl}/all/active`);
  }
}
