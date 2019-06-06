import {Component, OnInit, Input} from '@angular/core';
import {UserProfile} from '../../../models/responses/profile/userProfile';
import {ActivatedRoute} from '@angular/router';
import {UserInfoService} from '../../../services/user-info.service';
import {NgForm} from '@angular/forms';
import {Store} from '@ngrx/store';
import {State} from '../../../reducers/user-info.reducers';
import {EditUserProfileData} from '../../../models/requests/profile/editUserProfileData';
import {SuccessModalComponent} from '../../../modals/success-modal/success-modal.component';
import {MatDialog} from '@angular/material';
import {ErrorModalComponent} from '../../../modals/error-modal/error-modal.component';
import {SuccessData} from '../../../models/modalData/successData';
import {ExternalLoginService} from '../../../services/external-login.service';
import {LoginProvider} from '../../../models/enums/login-provider.enum';
import * as fromApp from '../../../reducers/app.reducers';
import * as UserInfoActions from '../../../actions/user-info.actions';
import * as BoardActions from '../../../actions/board.actions';

@Component({
    selector: 'app-profile-edit',
    templateUrl: './profile-edit.component.html',
    styleUrls: ['./profile-edit.component.scss']
})
export class ProfileEditComponent implements OnInit {
    @Input() profile: UserProfile;
    profileList: Store<State>;
    oldProfile: UserProfile;

    constructor(private route: ActivatedRoute,
                private userInfoService: UserInfoService, private dialog: MatDialog,
                private externalLoginService: ExternalLoginService,
                private store: Store<fromApp.AppState>) {
        this.profileList = this.store.select('userInfo');
    }

    ngOnInit() {
        this.profileList.subscribe(data => {
            this.profile = {
                firstName: this.profile.firstName,
                lastName: this.profile.lastName,
                email: this.profile.email,
                hashMail: this.profile.hashMail,
                isProfileAnonymous: this.profile.isProfileAnonymous
            };
            this.oldProfile = {...data.userProfile};
        });
    }

    onEditFormSubmit(form: NgForm) {
        if (form.invalid) {
            return false;
        }
        const newData: EditUserProfileData = {
            firstName: this.profile.firstName,
            lastName: this.profile.lastName
        };
        this.userInfoService.editUserProfile(newData).subscribe(() => {
            this.dialog.open(SuccessModalComponent, {
                data: SuccessData.default()
            });
            this.store.dispatch(new UserInfoActions.EditUserProfile(newData));
            this.store.dispatch(new BoardActions.EditBoardOwnerName(newData));
        }, err => {
            this.dialog.open(ErrorModalComponent);
        });
    }

    shouldDisable(form: NgForm) {
        if (form.invalid || !form.dirty) {
            return true;
        } else if (this.profile.firstName === this.oldProfile.firstName && this.profile.lastName === this.oldProfile.lastName) {
            return true;
        }
        return false;
    }

    isModificationAvailable() {
        return this.externalLoginService.isProviderAvailable(LoginProvider.Local);
    }
}
