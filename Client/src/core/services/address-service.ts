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
    return this.http.get<PaginatedResult<DropdownItem>>(`${this.baseUrl}/region/paginate`, {
      params: {
        search: params?.search ?? '',
        pageNumber: params?.pageNumber ?? 1,
        pageSize: params?.pageSize ?? 20,
      },
      context: new HttpContext().set(SkipLoading, true),
    });
  }

  getProvincesPaginated(regionId: number, params: DropDownParamsDTO | null) {
    return this.http.get<PaginatedResult<DropdownItem>>(
      `${this.baseUrl}/province/${regionId}/paginate`,
      {
        params: {
          search: params?.search ?? '',
          pageNumber: params?.pageNumber ?? 1,
          pageSize: params?.pageSize ?? 20,
        },
        withCredentials: true,
        context: new HttpContext().set(SkipLoading, true),
      },
    );
  }

  getCitiesMunicipalitiesPaginated(id: number, params: DropDownParamsDTO | null, byRegion = false) {
    return this.http.get<PaginatedResult<DropdownItem>>(
      `${this.baseUrl}/city-municipality/${id}/paginate`,
      {
        params: {
          search: params?.search ?? '',
          pageNumber: params?.pageNumber ?? 1,
          pageSize: params?.pageSize ?? 20,
          byRegion: byRegion,
        },
        withCredentials: true,
        context: new HttpContext().set(SkipLoading, true),
      },
    );
  }

  getBarangayPaginated(cityMunicipalityId: number, params: DropDownParamsDTO | null) {
    return this.http.get<PaginatedResult<DropdownItem>>(
      `${this.baseUrl}/barangay/${cityMunicipalityId}/paginate`,
      {
        params: {
          search: params?.search ?? '',
          pageNumber: params?.pageNumber ?? 1,
          pageSize: params?.pageSize ?? 20,
        },
        withCredentials: true,
        context: new HttpContext().set(SkipLoading, true),
      },
    );
  }
}
