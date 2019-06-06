import { Component, OnInit, Input } from '@angular/core';
import { ChangePasswordUserData } from '../../../../models/requests/profile/changePasswordData';
import { MatDialog } from '@angular/material';
import { NgForm, NgModel, FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { PasswordService } from '../password.service';
import { SuccessModalComponent } from '../../../../modals/success-modal/success-modal.component';
import { ErrorModalComponent } from '../../../../modals/error-modal/error-modal.component';
import { ConfirmPasswordValidator } from '../password-validator';
import { PASSWORD_REGEX, SPINNER_CONFIG } from '../../../../shared/constants';
import * as bowser from "bowser";

@Component({
    selector: 'app-password-change',
    templateUrl: './password-change.component.html',
    styleUrls: ['../password-forms.scss']
})
export class PasswordChangeComponent {
    isShowingForm = false;
    waitingForResponse = false;
    waitingForLocalization = false;

    passwordChangeForm: FormGroup;

    constructor(private fb: FormBuilder,
        private dialog: MatDialog,
        private passwordService: PasswordService) {
        this.passwordChangeForm = this.fb.group({
            oldPassword: ['', [Validators.required]],
            newPassword: ['', [Validators.required, Validators.pattern(PASSWORD_REGEX)]],
            confirmNewPassword: ['', [Validators.required]]
        }, {
                validator: ConfirmPasswordValidator.MatchNewPassword
            });
    }

    hidePasswordChangeForm() {
        this.isShowingForm = false;
        this.passwordChangeForm.reset();
    }

    showPasswordForm() {
        this.isShowingForm = true;
    }

    handleChangePasswordButton() {
        if (!this.isShowingForm) {
            this.showPasswordForm();
        } else {
            this.waitingForLocalization = true;
            this.changePassword();
        }
    }

    changePassword() {
        const userData: ChangePasswordUserData = {
            oldPassword: this.passwordChangeForm.get('oldPassword').value,
            newPassword: this.passwordChangeForm.get('newPassword').value,
            browser: bowser.name,
            browserVersion: bowser.version.toString()
        };
        this.passwordService.changePassword(userData).subscribe(() => {
            this.waitingForResponse = false;
            const popup = this.dialog.open(SuccessModalComponent, {
                data: {
                    smallMessage: 'Your password was successfully changed.'
                }
            });
            this.waitingForLocalization = false;
            this.hidePasswordChangeForm();
        }, (err) => {
            this.waitingForResponse = false;
            this.dialog.open(ErrorModalComponent, {
                data: {
                    smallMessage: 'Your old password is not correct.',
                    buttonName: 'Ok, try again later'
                }
            });
            this.waitingForLocalization = false;
        });
        this.waitingForResponse = true;
    }

    getSpinnerConfig() {
        return SPINNER_CONFIG;
    }
}
