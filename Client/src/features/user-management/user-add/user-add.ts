import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FlexRender } from '@tanstack/angular-table';
import { TextInput } from '../../../shared/forms/text-input/text-input';
import { DropdownRoleClientsideFilter } from '../../../shared/dropdown-paginated/dropdown-role-clientside-filter';
import { DropdownPaginate } from '../../../shared/dropdown-paginated/dropdown-paginate/dropdown-paginate';

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
    firstName: [''],
    middleName: [''],
    role: [null],
  });

  ngOnInit() {
    this.roleFilter.init();
  }

  addUser() {}
}
