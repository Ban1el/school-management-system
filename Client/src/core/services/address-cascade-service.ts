import { Injectable, inject, signal } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { DropdownBarangayFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-barangay-filter';
import { DropdownCityMunicipalityFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-city-municipality-filter';
import { DropdownProvinceFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-province-filter';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { DropdownRegionFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-region-filter';

const NCR_REGION_ID = 4;

// Minimal shape the service needs — doesn't care about your other fields
export interface AddressFormGroup extends FormGroup {
  controls: {
    region: FormControl<DropdownItem | null>;
    province: FormControl<DropdownItem | null>;
    cityMunicipality: FormControl<DropdownItem | null>;
    barangay: FormControl<DropdownItem | null>;
  };
}

@Injectable()
export class AddressCascadeService {
  public regionFilter = inject(DropdownRegionFilter);
  public provinceFilter = inject(DropdownProvinceFilter);
  public cityMunicipalityFilter = inject(DropdownCityMunicipalityFilter);
  public barangayFilter = inject(DropdownBarangayFilter);

  provinceHidden = signal(false);

  private form!: AddressFormGroup;

  attach(form: AddressFormGroup) {
    this.form = form;
    this.regionFilter.init();
  }

  onRegionSelected(item: DropdownItem | null) {
    if (item && item.id !== 0) {
      if (item.id === NCR_REGION_ID) {
        this.cityMunicipalityFilter.setId(item.id);
        this.cityMunicipalityFilter.init();

        this.provinceHidden.set(true);
        this.form.get('cityMunicipality')?.enable();
        this.form.get('barangay')?.disable();
      } else {
        this.provinceHidden.set(false);
        this.provinceFilter.setRegionId(item.id);
        this.provinceFilter.init();
        this.form.get('province')?.enable();
        this.form.get('cityMunicipality')?.disable();
        this.form.get('barangay')?.disable();
      }
    } else {
      this.provinceHidden.set(false);
      this.form.get('province')?.disable();
      this.form.get('cityMunicipality')?.disable();
      this.form.get('barangay')?.disable();
    }

    this.form.patchValue({ province: null, cityMunicipality: null, barangay: null });
  }

  onProvinceSelected(item: DropdownItem | null) {
    if (item && item.id !== 0) {
      this.cityMunicipalityFilter.setId(item.id);
      this.cityMunicipalityFilter.init();
      this.form.get('cityMunicipality')?.enable();
      this.form.get('barangay')?.disable();
    } else {
      this.form.get('cityMunicipality')?.disable();
      this.form.get('barangay')?.disable();
    }
    this.form.patchValue({ cityMunicipality: null, barangay: null });
  }

  onCityMunicipalitySelected(item: DropdownItem | null) {
    if (item && item.id !== 0) {
      this.barangayFilter.setCityMunicipalityId(item.id);
      this.barangayFilter.init();
      this.form.get('barangay')?.enable();
    } else {
      this.form.get('barangay')?.disable();
    }
    this.form.patchValue({ barangay: null });
  }
}
