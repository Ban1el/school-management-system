import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexRender } from '@tanstack/angular-table';
import { TextInput } from '../../../shared/forms/text-input/text-input';
import { DropdownRoleClientsideFilter } from '../../../shared/dropdown-paginated/dropdown-role-clientside-filter';
import { DropdownPaginate } from '../../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';
import { DropdownItem } from '../../../types/Dropdown/DropdownItemDto';

@Component({
  selector: 'app-user-add',
  imports: [FlexRender, ReactiveFormsModule, TextInput, DropdownPaginate],
  templateUrl: './user-add.html',
  styleUrl: './user-add.css',
  providers: [DropdownRoleClientsideFilter],
})
export class UserAdd {
  private fb = inject(FormBuilder);
  roleFilter = inject(DropdownRoleClientsideFilter);

  form = this.fb.group({
    userName: [''],
    firstName: [''],
    lastName: [''],
    middleName: [''],
    mobileNumber: [''],
    email: [''],
    role: [null],

    //Address
    region: [null as DropdownItem | null],
    province: [{ value: null as DropdownItem | null, disabled: true }],
    cityMunicipality: [{ value: null as DropdownItem | null, disabled: true }],
    barangay: [{ value: null as DropdownItem | null, disabled: true }],
    zipCode: [''],
    streetAddress: [''],
  });

  ngOnInit() {
    this.roleFilter.init();
  }

  addUser() {}
}
