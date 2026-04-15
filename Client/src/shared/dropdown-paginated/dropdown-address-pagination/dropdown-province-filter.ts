import { Injectable, inject, signal } from '@angular/core';
import { AddressService } from '../../../core/services/address-service';
import { DropdownItem } from '../../../types/Dropdown/DropdownItemDto';

@Injectable()
export class DropdownProvinceFilter {
  private addressService = inject(AddressService);

  items = signal<DropdownItem[]>([]);
  loading = signal(false);

  private page = 1;
  private pageSize = 20;
  private allLoaded = false;
  private search = '';
  private regionId = 0;

  init() {
    this.loadMore(true);
  }

  loadMore(reset = false) {
    if (reset) {
      this.page = 1;
      this.allLoaded = false;
      this.items.set([]);
    }

    if (this.loading() || this.allLoaded || this.regionId == 0) return;

    this.loading.set(true);
    this.addressService
      .getProvincesPaginated(this.regionId, {
        search: this.search,
        pageNumber: this.page,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (res) => {
          if (!res || res.items.length < this.pageSize) this.allLoaded = true;
          this.items.set([...this.items(), ...res.items]);
          this.page++;
        },
        complete: () => this.loading.set(false),
      });
  }

  onSearch(value: string) {
    this.search = value;
    this.loadMore(true);
  }

  setRegionId(id: number) {
    this.regionId = id;
  }
}
