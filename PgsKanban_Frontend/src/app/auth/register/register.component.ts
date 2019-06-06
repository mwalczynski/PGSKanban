import { RegisterResult } from './../../models/responses/auth/registerResult';
import { LoginComponent } from './../login/login.component';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { NgForm, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material';

import { RegisterService } from './register.service';
import { RegisterUserData } from './../../models/requests/auth/registerUserData';
import { SuccessModalComponent } from './../../modals/success-modal/success-modal.component';
import { Url } from '../../shared/constants';
import {SuccessData} from '../../models/modalData/successData';

@Component({
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
    isButtonDisabled: boolean;
    isCaptchaInvalid: boolean;
    isEmailInUse: boolean;
    isServerError: boolean;
    invalidEmail: String;
    data: RegisterUserData = new RegisterUserData();

    constructor(
        private router: Router,
        private dialog: MatDialog,
        private registerService: RegisterService
    ) { }

    ngOnInit() {
    }

    onRegisterFormSubmit(form: NgForm) {
        if (!this.canRegister(form)) {
            return false;
        }
        if (this.data.reCaptchaToken === '') {
            this.isCaptchaInvalid = true;
            return false;
        }

        this.isButtonDisabled = true;

        this.registerService.register(this.data).subscribe(result => {
            if (result.succeeded) {
                this.isServerError = false;
                this.dialog.open(SuccessModalComponent, { data: SuccessData.activationEmailSend() }).afterClosed().subscribe(() => {
                    this.isButtonDisabled = false;
                    this.router.navigate([Url.Login]);
                });
            }
        }, error => {
            if (error.status === 0) {
                this.isServerError = true;
            } else {
                const result = error.error as RegisterResult;
                this.isButtonDisabled = false;
                if (!result.reCaptchaValidated) {
                    this.isCaptchaInvalid = true;
                } else {
                    this.invalidEmail = this.data.email;
                    this.isEmailInUse = true;
                }
            }
        });
    }

    isEmailAlreadyUsed() {
        return this.invalidEmail === this.data.email && this.isEmailInUse;
    }

    canRegister(form: NgForm) {
        return form.valid && (this.data.password === this.data.confirmPassword) && !this.isButtonDisabled;
    }

    formInvalid(form: FormGroup) {
        if (form.touched && form.invalid) {
            return true;
        }
    }

    confirmPasswordInvalid(password: FormGroup, confirmPassword: FormGroup) {
        if (!confirmPassword.touched || password.invalid) {
            this.isButtonDisabled = true;
            return false;
        }
        if (this.data.password === this.data.confirmPassword) {
            this.isButtonDisabled = false;
            return false;
        }

        this.isButtonDisabled = true;
        return true;
    }

    emailInvalid(email: FormGroup) {
        return this.isEmailAlreadyUsed() || (email.touched && email.invalid);
    }
}
