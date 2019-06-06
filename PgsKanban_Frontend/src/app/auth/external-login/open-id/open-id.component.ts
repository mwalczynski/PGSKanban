import {Component, OnInit} from '@angular/core';
import {ExternalLoginData} from '../../../models/requests/auth/ExternalLoginData';
import {ActivatedRoute, Router} from '@angular/router';
import {ExternalLoginService} from '../../../services/external-login.service';
import { SPINNER_CONFIG, Url } from '../../../shared/constants';
import {AuthService} from '../../auth.service';
import {MatDialog} from '@angular/material';
import {ErrorModalComponent} from '../../../modals/error-modal/error-modal.component';
import { LoginProvider } from '../../../models/enums/login-provider.enum';

@Component({
    selector: 'app-open-id',
    templateUrl: '../external-login.html'
})
export class OpenIdComponent implements OnInit {
    externalLoginData: ExternalLoginData;
    provider: string;

    constructor(private route: ActivatedRoute, private externalLoginService: ExternalLoginService,
                private router: Router, private authService: AuthService, private dialog: MatDialog) {
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.externalLoginData = new ExternalLoginData(params['code'], params['state']);
        });
        this.route.params.subscribe(params => {
            this.provider = LoginProvider[this.route.snapshot.data['provider']];
        });
        this.authorizeUser();
    }

    authorizeUser() {
        this.externalLoginData.provider = this.provider;
        this.externalLoginService.authorizeUser(this.externalLoginData).subscribe(
            (response) => {
                this.authService.authorize(response.token);
                this.router.navigate([Url.Initial]);
            },
            () => {
                this.dialog.open(ErrorModalComponent).afterClosed()
                    .subscribe(() => {
                        this.router.navigate([Url.Login]);
                    });
            }
        );
    }

    getSpinnerConfig() {
        return SPINNER_CONFIG;
    }
}
