import { Component } from '@angular/core';
import {NavigationEnd, Router} from '@angular/router';
import 'rxjs/add/operator/filter';
import {PreviousRouteService} from './services/previous-route.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private router: Router, private previousStateService: PreviousRouteService) {
      this.router.events
          .filter((e: any) => e instanceof NavigationEnd)
          .subscribe((e: NavigationEnd) => {
              previousStateService.saveNavigation(e.url);
          });
  }
}
