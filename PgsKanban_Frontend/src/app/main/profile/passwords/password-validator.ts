import {AbstractControl} from '@angular/forms';

export class ConfirmPasswordValidator {
    static MatchNewPassword(AC: AbstractControl) {
        const newPassword = AC.get('newPassword').value; // to get value in input tag
        const confirmNewPassword = AC.get('confirmNewPassword').value; // to get value in input tag
        if (newPassword !== confirmNewPassword) {
            AC.get('confirmNewPassword').setErrors({MatchNewPassword: true});
        } else {
            return null;
        }
    }
}
