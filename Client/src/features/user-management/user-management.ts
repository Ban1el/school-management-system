import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserAuthService } from '../../core/services/user-auth-service';
import {
  FlexRender,
  injectTable,
  tableFeatures,
  createSortedRowModel,
  rowSortingFeature,
  sortFns,
  createTableHook,
} from '@tanstack/angular-table';
import type { ColumnDef } from '@tanstack/angular-table';

// 1. Define the shape of your data
type Person = {
  firstName: string;
  lastName: string;
  age: number;
};

// 2. Create some data with a stable reference
const defaultData: Array<Person> = [
  { firstName: 'tanner', lastName: 'linsley', age: 24 },
  { firstName: 'tandy', lastName: 'miller', age: 40 },
  { firstName: 'joe', lastName: 'dirte', age: 45 },
];

// 3. New in v9: declare which features this table uses (none yet)
const features = tableFeatures({
  rowSortingFeature, // enables sorting APIs and state
  sortedRowModel: createSortedRowModel(), // client-side sorting
  sortFns,
});

const { injectAppTable, createAppColumnHelper } = createTableHook({ features });

// 4. Define your columns
const columns: Array<ColumnDef<typeof features, Person>> = [
  {
    accessorKey: 'firstName', // accessorKey shorthand
    header: 'First Name',
    cell: (info) => info.getValue(),
  },
  {
    accessorFn: (row) => row.lastName, // accessorFn alternative with a custom id
    id: 'lastName',
    header: () => 'Last Name',
    cell: (info) => info.getValue<string>(),
  },
  {
    accessorKey: 'age',
    header: () => 'Age',
  },
];

@Component({
  selector: 'app-user-management',
  imports: [FlexRender, RouterLink],
  templateUrl: './user-management.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrl: './user-management.css',
})
export class UserManagement {
  protected userAuthService = inject(UserAuthService);
  private router = inject(Router);

  readonly data = signal<Array<Person>>([...defaultData]);

  // 6. Create the table instance
  readonly table = injectTable(() => ({
    key: 'person-table',
    features,
    columns,
    data: this.data(),
  }));

  sortIndicator(sortDirection: false | 'asc' | 'desc') {
    if (sortDirection === 'asc') return ' 🔼';
    if (sortDirection === 'desc') return ' 🔽';
    return null;
  }
}
