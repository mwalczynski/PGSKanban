import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PreviousRouteService } from '../../services/previous-route.service';
import { Location } from '@angular/common';
import { Url } from '../../shared/constants';

@Component({
    selector: 'app-error404',
    templateUrl: './error404.component.html',
    styleUrls: ['./error404.component.scss']
})
export class Error404Component {

    constructor(private router: Router, private location: Location, private previousRouteService: PreviousRouteService) { }

    redirectBack(): void {
        if (this.previousRouteService.checkIfNavigatedBefore()) {
            this.location.back();
        } else {
            this.router.navigate([Url.Login]);
        }
    }
}
