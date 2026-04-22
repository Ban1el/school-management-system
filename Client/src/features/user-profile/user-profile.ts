import { Component, inject, OnInit, signal } from '@angular/core';
import { AddressService } from '../../core/services/address-service';
import { DropdownRegionFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-region-filter';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { DropdownPaginate } from '../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { DropdownProvinceFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-province-filter';
import { DropdownCityMunicipalityFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-city-municipality-filter';
import { DropdownBarangayFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-barangay-filter';
import { GenderService } from '../../core/services/gender-service';
import { GenderDto } from '../../types/Gender/GenderDto';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-user-profile',
  imports: [DropdownPaginate, ReactiveFormsModule, NgClass],
  providers: [
    DropdownRegionFilter,
    DropdownProvinceFilter,
    DropdownCityMunicipalityFilter,
    DropdownBarangayFilter,
  ],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {
  regionFilter = inject(DropdownRegionFilter);
  provinceFilter = inject(DropdownProvinceFilter);
  cityMunicipalityFilter = inject(DropdownCityMunicipalityFilter);
  barangayFilter = inject(DropdownBarangayFilter);
  genderService = inject(GenderService);

  protected isEdit = signal(false);

  protected genders = signal<GenderDto[]>([]);
  provinceHidden = signal(false);

  ngOnInit(): void {
    this.genderService.getGendersActive().subscribe({
      next: (result) => {
        this.genders.set(result);
      },
      error: (err) => {},
      complete: () => {},
    });
    this.regionFilter.init();
  }

  form = new FormGroup({
    region: new FormControl(null),
    province: new FormControl({ value: null, disabled: true }),
    cityMunicipality: new FormControl({ value: null, disabled: true }),
    barangay: new FormControl({ value: null, disabled: true }),
  });

  onRegionSelected(item: DropdownItem | null) {
    if (item && item.id !== 0) {
      if (item.id == 4) {
        //Enter here when NCR is selected
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
