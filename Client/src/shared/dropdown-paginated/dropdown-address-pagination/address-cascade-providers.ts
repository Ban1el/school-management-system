import { Provider } from '@angular/core';
import { DropdownRegionFilter } from './dropdown-region-filter';
import { DropdownProvinceFilter } from './dropdown-province-filter';
import { DropdownCityMunicipalityFilter } from './dropdown-city-municipality-filter';
import { DropdownBarangayFilter } from './dropdown-barangay-filter';
import { AddressCascadeService } from '../../../core/services/address-cascade-service';

export function AddressCascadeProviders(): Provider[] {
  return [
    DropdownRegionFilter,
    DropdownProvinceFilter,
    DropdownCityMunicipalityFilter,
    DropdownBarangayFilter,
    AddressCascadeService,
  ];
}
