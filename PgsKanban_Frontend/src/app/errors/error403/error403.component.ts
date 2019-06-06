import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Url } from '../../shared/constants';

@Component({
    selector: 'app-error403',
    templateUrl: './error403.component.html',
    styleUrls: ['./error403.component.scss']
})
export class Error403Component {

    constructor(private router: Router) { }

    redirectToMainPage(): void {
        this.router.navigate([Url.Login]);
    }
}
