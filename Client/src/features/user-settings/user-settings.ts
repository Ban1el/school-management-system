import { Component, inject } from '@angular/core';
import { TextInput } from '../../shared/forms/text-input/text-input';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-settings',
  imports: [TextInput, ReactiveFormsModule],
  templateUrl: './user-settings.html',
  styleUrl: './user-settings.css',
})
export class UserSettings {
  private fb = inject(FormBuilder);

  form = this.fb.group({
    password: [''],
  });

  updatePassword() {}
}
