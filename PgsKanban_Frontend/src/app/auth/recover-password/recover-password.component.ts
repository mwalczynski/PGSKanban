import { Url } from './../../shared/constants';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validator } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';

import { AuthService } from './../auth.service';
import { ResetPasswordData } from '../../models/requests/profile/resetPasswordData';
import { RecoverService } from './recover.service';
import { SuccessModalComponent } from './../../modals/success-modal/success-modal.component';
import { ErrorModalComponent } from './../../modals/error-modal/error-modal.component';
import { ErrorData } from '../../models/modalData/errorData';

@Component({
    templateUrl: './recover-password.component.html',
    styleUrls: ['./recover-password.component.scss']
})
export class RecoverPasswordComponent implements OnInit {
    disableButton: boolean;
    token: any;
    data: ResetPasswordData = new ResetPasswordData();

    constructor(private recoverService: RecoverService, private route: ActivatedRoute,
        private router: Router, public dialog: MatDialog, private authService: AuthService) {

        this.route.params.subscribe(params => {
            this.token = params['token'];
        });
    }

    ngOnInit() {
        this.authService.removeToken();

        this.recoverService.validateToken(this.token).subscribe(data => {
            const resetTokenValidationResponse = data;
            switch (true) {
                case resetTokenValidationResponse.expired:
                    this.dialog.open(ErrorModalComponent, {
                        data: this.showPopup(
                            'Your reset password link has expired',
                            'Ok, try again')
                    }).afterClosed().subscribe(() => {
                        this.router.navigate([Url.Login]);
                    });
                    break;
                case resetTokenValidationResponse.invalid:
                    this.dialog.open(ErrorModalComponent, {
                        data: this.showPopup(
                            'Your reset password link is not correct',
                            'Ok, try again')
                    }).afterClosed().subscribe(() => {
                        this.router.navigate([Url.Login]);
                    });
                    break;
            }
        });
    }

    onRecoverFormSubmit(form: FormGroup) {
        if (form.invalid) {
            return false;
        }
        this.data.token = this.token;
        this.disableButton = true;
        this.recoverService.changePassword(this.data).subscribe(() => {
            this.disableButton = false;
            this.dialog.open(SuccessModalComponent).afterClosed().subscribe(() => {
                this.router.navigate([Url.Login]);
            });
        }, (error) => {
            this.disableButton = false;
            this.dialog.open(ErrorModalComponent, {
                data: this.showPopup(
                    'Your password was not changed',
                    'Ok, try again later')
            }).afterClosed().subscribe(() => {
                this.router.navigate([Url.Login]);
            });
        });
    }

    passwordInvalid(form: FormGroup) {
        if (form.touched && form.invalid) {
            return true;
        }
    }

    confirmPasswordInvalid(form: FormGroup) {
        if (!form.touched || form.invalid) {
            this.disableButton = true;
            return false;
        }
        if (this.data.password !== this.data.confirmPassword) {
            this.disableButton = true;
            return true;
        }
        this.disableButton = false;
        return false;
    }

    showPopup(smallMessage: string, buttonName: string) {
        return new ErrorData(smallMessage, buttonName);
    }
}
