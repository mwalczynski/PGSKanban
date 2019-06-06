import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {PasswordService} from '../password.service';
import {MatDialog} from '@angular/material';
import {ConfirmPasswordValidator} from '../password-validator';
import {AddPasswordUserData} from '../../../../models/requests/profile/addPasswordData';
import {SuccessModalComponent} from '../../../../modals/success-modal/success-modal.component';
import {SuccessData} from '../../../../models/modalData/successData';
import {ErrorModalComponent} from '../../../../modals/error-modal/error-modal.component';
import {PASSWORD_REGEX} from '../../../../shared/constants';

@Component({
    selector: 'app-password-add',
    templateUrl: './password-add.component.html',
    styleUrls: ['../password-forms.scss']
})
export class PasswordAddComponent {
    @Output() addedPassword = new EventEmitter();
    isShowingForm = false;
    waitingForResponse = false;

    passwordAddForm: FormGroup;

    constructor(private fb: FormBuilder,
                private dialog: MatDialog,
                private passwordService: PasswordService) {
        this.passwordAddForm = this.fb.group({
            newPassword: ['', [Validators.required, Validators.pattern(PASSWORD_REGEX)]],
            confirmNewPassword: ['', [Validators.required]]
        }, {
            validator: ConfirmPasswordValidator.MatchNewPassword
        });
    }

    hidePasswordAddForm() {
        this.isShowingForm = false;
        this.passwordAddForm.reset();
    }

    showPasswordForm() {
        this.isShowingForm = true;
    }

    handleAddPasswordButton() {
        if (!this.isShowingForm) {
            this.showPasswordForm();
        } else {
            this.addPassword();
        }
    }

    addPassword() {
        const userData: AddPasswordUserData = new AddPasswordUserData(
                this.passwordAddForm.get('newPassword').value,
                this.passwordAddForm.get('confirmNewPassword').value
        );
        this.passwordService.addPassword(userData).subscribe(() => {
            this.waitingForResponse = false;
            this.addedPassword.emit();
            this.dialog.open(SuccessModalComponent, {
                data: new SuccessData('Password has been successfully added.')
            });
            this.hidePasswordAddForm();
        }, () => {
            this.waitingForResponse = false;
            this.passwordAddForm.reset();
            this.dialog.open(ErrorModalComponent);
        });
        this.waitingForResponse = true;
    }

}
