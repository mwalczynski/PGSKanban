import {Url} from './../../shared/constants';
import {Component, OnInit} from '@angular/core';
import {UserProfile} from '../../models/responses/profile/userProfile';
import {ActivatedRoute, Router} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import * as fromApp from '../../reducers/app.reducers';
import {Store} from '@ngrx/store';

@Component({
    selector: 'app-user-info',
    templateUrl: './user-info.component.html',
    styleUrls: ['./user-info.component.scss']
})
export class UserInfoComponent implements OnInit {
    profileList: Observable<any>;
    profile: UserProfile;
    firstName: string;
    lastName; string;
    imageSrc: string;
    activeDropdown = false;

    constructor(private router: Router,
                private route: ActivatedRoute,
                private store: Store<fromApp.AppState>) {
        this.profileList = this.store.select('userInfo');
    }

    ngOnInit() {
        this.profileList.subscribe(data => {
            this.profile = {...data.userProfile};
            if (this.profile.pictureSrc) {
                this.imageSrc = this.profile.pictureSrc;
            } else {
                this.imageSrc = `https://www.gravatar.com/avatar/${this.profile.hashMail}?s=40`;
            }
        });
    }

    toggleDropdown(): void {
        this.activeDropdown = !this.activeDropdown;
    }

    hideDropdown(): void {
        this.activeDropdown = false;
    }

    goToProfile(): void {
        this.router.navigate([Url.Profile]);
    }
}
