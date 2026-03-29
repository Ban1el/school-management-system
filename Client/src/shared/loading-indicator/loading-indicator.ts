import { Component, ContentChild, Input, OnInit, TemplateRef } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { LoadingService } from '../../core/services/loading-service';
import { RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';
import { AsyncPipe, NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-loading-indicator',
  imports: [AsyncPipe, NgTemplateOutlet],
  templateUrl: './loading-indicator.html',
  styleUrl: './loading-indicator.css',
})
export class LoadingIndicator implements OnInit {
  loading$: Observable<boolean>;

  @Input() detectRouteTransition = false;

  @ContentChild('loading') customLoadingIndicator: TemplateRef<any> | null = null;

  constructor(
    private loadingService: LoadingService,
    private router: Router,
  ) {
    this.loading$ = this.loadingService.loading$;
  }
  ngOnInit(): void {
    if (this.detectRouteTransition) {
      this.router.events
        .pipe(
          tap((event) => {
            if (event instanceof RouteConfigLoadStart) {
              this.loadingService.loadingOn();
            } else if (event instanceof RouteConfigLoadEnd) {
              this.loadingService.loadingOff();
            }
          }),
        )
        .subscribe();
    }
  }
}
