import { Component, OnInit, Input } from '@angular/core';
import { UserInfoService } from '../../services/user-info.service';
import { AuthService } from '../../auth/auth.service';
import { UserProfile } from '../../models/responses/profile/userProfile';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { EditUserProfileData } from '../../models/requests/profile/editUserProfileData';
import { ExternalLoginService } from '../../services/external-login.service';
import { LoginProvider } from '../../models/enums/login-provider.enum';
import { SharedService } from './../shared.service';

import * as fromApp from '../../reducers/app.reducers';
import { Store } from '@ngrx/store';
import { State } from '../../reducers/user-info.reducers';

@Component({
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
    profileList: Store<State>;
    profile: UserProfile;

    constructor(
        private route: ActivatedRoute,
        private externalLoginService: ExternalLoginService,
        private sharedService: SharedService,
        private store: Store<fromApp.AppState>) {
        this.profileList = this.store.select('userInfo');
    }

    ngOnInit() {
        this.profileList.subscribe(data => this.profile = data.userProfile);
    }

    shouldBeChangePasswordFormVisible() {
        return this.externalLoginService.isProviderAvailable(LoginProvider.Local) && this.profile.hasPassword;
    }

    shouldBeAddPasswordFormVisible() {
        return this.externalLoginService.isProviderAvailable(LoginProvider.Local) && !this.profile.hasPassword;
    }

    userAddedPassword() {
        this.profile.hasPassword = true;
    }

    onClickOutsideSidebar() {
        this.sharedService.foldSidebars();
    }
}
