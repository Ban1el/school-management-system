import { Component, inject, OnInit, signal } from '@angular/core';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { DropdownPaginate } from '../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { GenderService } from '../../core/services/gender-service';
import { GenderDto } from '../../types/Gender/GenderDto';
import { UserService } from '../../core/services/user-service';
import { UserDto } from '../../types/User/UserDto';
import { UserAuthService } from '../../core/services/user-auth-service';
import { UserProfileUpdateDto } from '../../types/User/UserProfileUpdateDto';
import { ToastService } from '../../core/services/toast-service';
import { TextInput } from '../../shared/forms/text-input/text-input';
import { NCR_NAME } from '../../core/constants/AddressConstants';
import {
  AddressCascadeService,
  AddressFormGroup,
} from '../../core/services/address-cascade-service';
import { DropdownBarangayFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-barangay-filter';
import { DropdownCityMunicipalityFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-city-municipality-filter';
import { DropdownProvinceFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-province-filter';
import { DropdownRegionFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-region-filter';
import { AddressCascadeProviders } from '../../shared/dropdown-paginated/dropdown-address-pagination/address-cascade-providers';

@Component({
  selector: 'app-user-profile',
  imports: [DropdownPaginate, ReactiveFormsModule, TextInput],
  providers: [AddressCascadeProviders(), AddressCascadeService],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {
  toastService = inject(ToastService);
  userAuthService = inject(UserAuthService);
  userService = inject(UserService);
  genderService = inject(GenderService);
  addressCascade = inject(AddressCascadeService);

  protected isEdit = signal(false);
  protected genders = signal<GenderDto[]>([]);
  protected user = signal<UserDto | null>(null);
  protected userId = this.userAuthService.user()?.id ?? 0;

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
        this.setForm();
      },
    });

    this.addressCascade.attach(this.form as AddressFormGroup);
    this.form.disable();
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
    email: ['', Validators.email],
    gender: [0],
    region: [null as DropdownItem | null],
    province: [{ value: null as DropdownItem | null, disabled: true }],
    cityMunicipality: [{ value: null as DropdownItem | null, disabled: true }],
    barangay: [{ value: null as DropdownItem | null, disabled: true }],
    zipCode: [''],
    streetAddress: [''],
  });

  updateUser() {
    if (!this.form.valid) {
      this.form.markAllAsTouched();
      return;
    }

    const { region, province, cityMunicipality, barangay, gender, ...rest } = this.form.value;

    const dto: UserProfileUpdateDto = {
      email: rest.email ?? undefined,
      firstName: rest.firstName ?? undefined,
      middleName: rest.middleName ?? undefined,
      lastName: rest.lastName ?? undefined,
      mobileNumber: rest.mobileNumber ?? undefined,
      streetAddress: rest.streetAddress ?? undefined,
      zipCode: rest.zipCode ?? undefined,
      regionId: region?.id ?? undefined,
      provinceId: province?.id ?? undefined,
      cityMunicipalityId: cityMunicipality?.id ?? undefined,
      barangayId: barangay?.id ?? undefined,
    };

    this.userService.updateUser(this.userId, dto).subscribe({
      next: () => {},
      error: (err) => {
        const errors = err.error as string[];
        this.toastService.error(errors.join('\n'));
      },
      complete: () => {},
    });
  }

  setForm() {
    const user = this.user();

    if (user) {
      if (user.regionName == NCR_NAME) {
        this.addressCascade.provinceHidden.set(true);
      }

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
    this.addressCascade.onRegionSelected(item);
  }
  onProvinceSelected(item: DropdownItem | null) {
    this.addressCascade.onProvinceSelected(item);
  }
  onCityMunicipalitySelected(item: DropdownItem | null) {
    this.addressCascade.onCityMunicipalitySelected(item);
  }
}
