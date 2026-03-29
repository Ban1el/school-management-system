import { Component, inject } from '@angular/core';
import { UserAuthService } from '../../core/services/user-auth-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.html',
  styleUrl: './user-management.css',
})
export class UserManagement {
  protected userAuthService = inject(UserAuthService);
  private router = inject(Router);

  logout() {
    this.userAuthService.logout().subscribe({
      complete: () => {
        this.router.navigateByUrl('/');
      },
    });
  }
}
