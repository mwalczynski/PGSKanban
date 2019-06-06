import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';
import {Router} from '@angular/router';

import {RequestPasswordResetData} from '../../models/requests/profile/requestPasswordResetData';
import {RecoveryPasswordResult} from '../../models/responses/auth/recoveryPasswordResult';
import {ResetPasswordService} from './reset-password.service';
import {MatDialog} from '@angular/material';
import {SuccessModalComponent} from '../../modals/success-modal/success-modal.component';
import {Url} from '../../shared/constants';

@Component({
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
    disableButton: boolean;
    isCaptchaDisplayed: boolean;
    data: RequestPasswordResetData = new RequestPasswordResetData();

    constructor(private resetPasswordService: ResetPasswordService, private router: Router, public dialog: MatDialog) {
    }

    ngOnInit() {
        this.shouldCaptchaBeDisplayed();
    }

    shouldCaptchaBeDisplayed() {
        this.resetPasswordService.isCaptchaDisplayed().subscribe((isCaptchaDisplayed) => {
            this.isCaptchaDisplayed = isCaptchaDisplayed;
        });
    }

    onResetFormSubmit(form: FormGroup) {
        this.disableButton = true;
        this.resetPasswordService.resetPasswordRequest(this.data).subscribe(() => {
            this.disableButton = false;
            this.dialog.open(SuccessModalComponent).afterClosed().subscribe(() => {
                this.router.navigate([Url.Login]);
            });
        }, (error) => {
            const data = <RecoveryPasswordResult>error;
            this.handleCaptcha(data.isCaptchaDisplayed, form.controls.captcha);
            this.disableButton = false;
        });
    }

    shouldChangeColorOfEmailInput(email: FormControl) {
        return (email.invalid && email.touched ? 'inputField--redError' : '');
    }

    handleCaptcha(showCaptcha: boolean, captcha) {
        this.isCaptchaDisplayed = showCaptcha;
        if (this.isCaptchaDisplayed && captcha) {
            captcha.reset();
        }
    }

}
