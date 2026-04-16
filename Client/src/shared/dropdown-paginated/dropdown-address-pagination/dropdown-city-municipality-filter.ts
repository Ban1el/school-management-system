import { Injectable, inject, signal } from '@angular/core';
import { AddressService } from '../../../core/services/address-service';
import { DropdownItem } from '../../../types/Dropdown/DropdownItemDto';

@Injectable()
export class DropdownCityMunicipalityFilter {
  private addressService = inject(AddressService);

  items = signal<DropdownItem[]>([]);
  loading = signal(false);

  private page = 1;
  private pageSize = 20;
  private allLoaded = false;
  private search = '';
  private id = 0;
  private isNcr = false;

  init() {
    this.loadMore(true);
  }

  loadMore(reset = false) {
    if (reset) {
      this.page = 1;
      this.allLoaded = false;
      this.items.set([]);
    }

    if (this.loading() || this.allLoaded || this.id == 0) return;

    //NCR = 4
    if (this.id == 4) this.isNcr = true;
    else this.isNcr = false;

    this.loading.set(true);
    this.addressService
      .getCitiesMunicipalitiesPaginated(
        this.id,
        {
          search: this.search,
          pageNumber: this.page,
          pageSize: this.pageSize,
        },
        this.isNcr,
      )
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

  setId(id: number) {
    this.id = id;
  }
}
