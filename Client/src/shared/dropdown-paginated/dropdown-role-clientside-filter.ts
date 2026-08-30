import { Injectable, inject, signal } from '@angular/core';
import { RoleService } from '../../core/services/role-service';
import { DropdownItem } from '../../types/Dropdown/DropdownItemDto';

@Injectable()
export class DropdownRoleClientsideFilter {
  private roleService = inject(RoleService);

  items = signal<DropdownItem[]>([]);
  loading = signal(false);

  private allItems: DropdownItem[] = [];
  private loaded = false;

  private page = 1;
  private pageSize = 20;
  private search = '';

  init() {
    if (this.loaded) {
      this.loadMore(true);
      return;
    }

    this.loading.set(true);
    this.roleService.getRoles().subscribe({
      next: (res) => {
        this.allItems = res ?? [];
        this.loaded = true;
        this.loadMore(true);
      },
      complete: () => this.loading.set(false),
    });
  }

  loadMore(reset = false) {
    if (reset) this.page = 1;

    const term = this.search.toLowerCase().trim();
    const filtered = term
      ? this.allItems.filter((i) => i.name.toLowerCase().includes(term))
      : this.allItems;

    const slice = filtered.slice(0, this.page * this.pageSize);
    this.items.set(slice);

    if (slice.length < filtered.length) this.page++;
  }

  onSearch(value: string) {
    this.search = value;
    this.loadMore(true);
  }
}
