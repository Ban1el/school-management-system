import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { UserAuthService } from '../../core/services/user-auth-service';

@Component({
  selector: 'app-nav',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected userAuthService = inject(UserAuthService);
  private router = inject(Router);

  isSidebarOpen = true;

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  logout() {
    this.userAuthService.logout().subscribe({
      next: () => {
        this.router.navigateByUrl('/');
      },
    });
  }
}
