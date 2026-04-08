import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpContext } from '@angular/common/http';
import { RegionDto } from '../../types/Address/RegionDto';
import { DropDownParamsDTO } from '../../types/Dropdown/DropdownDto';
import { SkipLoading } from '../interceptors/loading-interceptor';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { PaginatedResult } from '../../types/Pagination/Pagination';

@Injectable({
  providedIn: 'root',
})
export class AddressService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}address`;

  getRegions() {
    return this.http.get<RegionDto[]>(`${this.baseUrl}/region/all`);
  }

  getRegionsPaginated(params: DropDownParamsDTO | null) {
    return this.http.post<PaginatedResult<DropdownItem>>(
      `${this.baseUrl}/region/all/paginate`,
      params,
      {
        withCredentials: true,
        context: new HttpContext().set(SkipLoading, true),
      },
    );
  }
}
