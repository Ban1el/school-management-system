import { Component, inject, OnInit } from '@angular/core';
import { AddressService } from '../../core/services/address-service';
import { DropdownRegionFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-region-filter';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { DropdownPaginate } from '../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { DropdownProvinceFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-province-filter';

@Component({
  selector: 'app-user-profile',
  imports: [DropdownPaginate, ReactiveFormsModule],
  providers: [DropdownRegionFilter, DropdownProvinceFilter],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {
  regionFilter = inject(DropdownRegionFilter);
  provinceFilter = inject(DropdownProvinceFilter);

  ngOnInit(): void {
    this.regionFilter.init();
    this.form.get('province')?.disable();
  }

  form = new FormGroup({
    region: new FormControl(null),
    province: new FormControl({ value: null, disabled: true }), // disabled by default
    city: new FormControl({ value: null, disabled: true }),
  });

  onRegionSelected(item: DropdownItem | null) {
    if (item && item.id !== 0) {
      this.provinceFilter.setRegionId(item.id);
      this.provinceFilter.init();
      this.form.get('province')?.enable(); // unlock province
      this.form.get('city')?.disable(); // reset city
      this.form.patchValue({ province: null, city: null });
    } else {
      this.form.get('province')?.disable();
      this.form.get('city')?.disable();
    }
  }

  // onProvinceSelected(item: DropdownItem | null) {
  //   if (item && item.id !== 0) {
  //     this.cityFilter.setProvinceId(item.id);
  //     this.cityFilter.init();
  //     this.form.get('city')?.enable(); // unlock city
  //     this.form.patchValue({ city: null });
  //   } else {
  //     this.form.get('city')?.disable();
  //   }
  // }
}
