import { Component, inject, OnInit } from '@angular/core';
import { AddressService } from '../../core/services/address-service';
import { DropdownRegionFilter } from '../../shared/dropdown-paginated/dropdown-address-pagination/dropdown-region-filter';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';
import { DropdownPaginate } from '../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';

@Component({
  selector: 'app-user-profile',
  imports: [DropdownPaginate],
  providers: [DropdownRegionFilter],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {
  regionFilter = inject(DropdownRegionFilter);

  onRegionSelected(item: DropdownItem | null) {
    console.log('selected region', item);
  }

  ngOnInit(): void {
    this.regionFilter.init();
  }
}
