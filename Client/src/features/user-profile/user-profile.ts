import { Component, inject, OnInit, signal } from '@angular/core';
import { AddressService } from '../../core/services/address-service';
import { DropdownRegionFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-region-filter';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { DropdownPaginate } from '../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';
import { FormGroup, FormControl, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { DropdownProvinceFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-province-filter';
import { DropdownCityMunicipalityFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-city-municipality-filter';
import { DropdownBarangayFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-barangay-filter';
import { GenderService } from '../../core/services/gender-service';
import { GenderDto } from '../../types/Gender/GenderDto';
import { NgClass } from '@angular/common';
import { UserService } from '../../core/services/user-service';
import { UserDto } from '../../types/User/UserDto';
import { UserAuthService } from '../../core/services/user-auth-service';

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
  userAuthService = inject(UserAuthService);
  userService = inject(UserService);
  regionFilter = inject(DropdownRegionFilter);
  provinceFilter = inject(DropdownProvinceFilter);
  cityMunicipalityFilter = inject(DropdownCityMunicipalityFilter);
  barangayFilter = inject(DropdownBarangayFilter);
  genderService = inject(GenderService);

  protected isEdit = signal(false);
  protected genders = signal<GenderDto[]>([]);
  protected user = signal<UserDto | null>(null);
  protected userId = this.userAuthService.user()?.id ?? 0;
  provinceHidden = signal(false);

  private fb = inject(FormBuilder);

  ngOnInit(): void {
    this.genderService.getGendersActive().subscribe({
      next: (result) => {
        this.genders.set(result);
      },
    });

    this.userService.getUser(this.userId).subscribe({
      next: (result) => {
        this.user.set(result);
        console.log(this.user());
      },
    });

    this.regionFilter.init();
    this.form.disable();
    this.setForm();
  }

  toggleEdit() {
    this.isEdit.set(!this.isEdit());

    if (this.isEdit()) {
      this.form.enable();
    } else {
      this.setForm();
      this.form.disable();
    }
  }

  form = this.fb.group({
    firstName: [''],
    middleName: [''],
    lastName: [''],
    mobileNumber: [''],
    email: [''],
    gender: [0],
    region: [null as DropdownItem | null],
    province: [{ value: null as DropdownItem | null, disabled: true }],
    cityMunicipality: [{ value: null as DropdownItem | null, disabled: true }],
    barangay: [{ value: null as DropdownItem | null, disabled: true }],
    zipCode: [''],
    streetAddress: [''],
  });

  setForm() {
    const user = this.user();
    if (user) {
      this.form.patchValue({
        firstName: user.firstName ?? '',
        middleName: user.middleName ?? '',
        lastName: user.lastName ?? '',
        mobileNumber: user.mobileNumber ?? '',
        email: user.email ?? '',
        region: user.regionId ? { id: user.regionId, name: user.regionName ?? '' } : null,
        province: user.provinceId ? { id: user.provinceId, name: user.provinceName ?? '' } : null,
        cityMunicipality: user.cityMunicipalityId
          ? { id: user.cityMunicipalityId, name: user.cityMunicipalityName ?? '' }
          : null,
        barangay: user.barangayId ? { id: user.barangayId, name: user.barangayName ?? '' } : null,
        zipCode: user.zipCode ?? '',
        streetAddress: user.streetAddress ?? '',
      });
    }
  }

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
