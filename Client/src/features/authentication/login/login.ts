import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../../core/services/toast-service';
import { UserAuthService } from '../../../core/services/user-auth-service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  protected userAuthService = inject(UserAuthService);
  protected toastService = inject(ToastService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  loginForm: FormGroup;

  constructor() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  login() {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const formData = { ...this.loginForm.value };
    this.userAuthService.login(formData).subscribe({
      next: () => {
        this.router.navigateByUrl('/user/management');
        this.toastService.success('Login successful!');
      },
      error: (err) => {},
      complete: () => {},
    });
  }
}
