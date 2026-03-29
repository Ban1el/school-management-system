import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingIndicator } from '../shared/loading-indicator/loading-indicator';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, LoadingIndicator],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('Client');
}
