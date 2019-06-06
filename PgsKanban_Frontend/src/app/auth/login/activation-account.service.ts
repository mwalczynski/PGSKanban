import {Injectable} from '@angular/core';
import {Http} from '@angular/http';
import {ConfirmEmailData} from '../../models/requests/auth/confirmEmailData';
import {BASE_URL, Url} from '../../shared/constants';
import {Observable} from 'rxjs/Observable';
import {ConfirmationEmailResult} from '../../models/responses/auth/confirmationEmailResult';
import {MatDialog} from '@angular/material';
import {SuccessModalComponent} from '../../modals/success-modal/success-modal.component';
import {SuccessData} from '../../models/modalData/successData';
import {ErrorModalComponent} from '../../modals/error-modal/error-modal.component';
import {ErrorData} from '../../models/modalData/errorData';
import {Router} from '@angular/router';
import {ResendEmailConfirmationData} from '../../models/requests/auth/resendEmailConfirmationData';
import {Location} from '@angular/common';

@Injectable()
export class ActivationAccountService {
    token: string;
    email: string;

    constructor(private http: Http, private dialog: MatDialog, private router: Router, private location: Location) {
    }

    confirmAccount(confirmationToken: ConfirmEmailData): Observable<ConfirmationEmailResult> {
        return this.http.post(`${BASE_URL}/account/confirmation`, confirmationToken).map(response => response.json());
    }

    handleActivation(emailConfirmResult: ConfirmationEmailResult) {
        if (emailConfirmResult === null) {
            this.showSuccessMessage(SuccessData.activated());
        } else {
            this.token = emailConfirmResult.token;
            switch (true) {
                case emailConfirmResult.expired:
                    this.showExpiredActivationTokenPopup();
                    break;
                case emailConfirmResult.invalid:
                    this.showInvalidActivationTokenPopup();
                    break;
                case emailConfirmResult.alreadyConfirmed:
                    this.showAlreadyActivatedTokenPopup();
                    break;
            }
        }
    }

    showExpiredActivationTokenPopup() {
        this.showErrorMessage(ErrorData.expiredActivation(this.resendActivationEmail));
    }

    resendActivationEmail = () => {
        return this.http.post(`${BASE_URL}/account/resendConfirmation`,
            new ConfirmEmailData(this.token)).map(response => response.json())
            .subscribe(
                this.showSuccessMessage,
                this.showErrorMessage
            );
    };

    showInvalidActivationTokenPopup() {
        this.showErrorMessage(ErrorData.invalidActivation());
    }

    showAlreadyActivatedTokenPopup() {
        this.showSuccessMessage(SuccessData.alreadyActivated());
    }

    showNotActivatedAccountPopup(email) {
        this.email = email;
        this.showErrorMessage(ErrorData.notActivated(this.resendActivationByEmail));
    }

    resendActivationByEmail = () => {
        return this.http.post(`${BASE_URL}/account/resend`, new ResendEmailConfirmationData(this.email)).subscribe(
            this.showSuccessMessage,
            this.showErrorMessage
        );
    }

    showSuccessMessage = (successData?) => {
        if (!(successData instanceof SuccessData)) {
            successData = null;
        }
        this.dialog.open(SuccessModalComponent, {data: successData})
            .afterClosed().subscribe(this.navigateToLogin);
    }

    showErrorMessage = (errorData?) => {
        if (!(errorData instanceof ErrorData)) {
            errorData = null;
        }
        this.dialog.open(ErrorModalComponent, {data: errorData})
            .afterClosed().subscribe(this.navigateToLogin);
    }

    navigateToLogin = () => {
        this.location.go(this.router.createUrlTree([Url.Login]).toString());
    }
}
