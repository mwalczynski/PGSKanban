import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserInfoService } from '../../../services/user-info.service';
import * as fromApp from '../../../reducers/app.reducers';
import * as StatisticsActions from '../../../actions/statistics.actions';
import { UserProfile } from '../../../models/responses/profile/userProfile';
import { Statistics } from '../../../models/responses/profile/statistics';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';

@Component({
    selector: 'app-welcome',
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent implements OnInit {
    userData: UserProfile;
    statisticsInfo: Statistics;

    @Output() lastBoardDeleted = new EventEmitter();

    constructor(private router: Router,
                private userInfoService: UserInfoService,
                private store: Store<fromApp.AppState>) {
    }

    ngOnInit() {
        this.getData();
        this.getStatistics();
        this.store.dispatch(new StatisticsActions.StatisticsUpdated());
        this.store.select('statisticsShouldBeUpdated').subscribe(state => {
            if (state.shouldStatisticsBeUpdated) {
                this.getStatistics();
            }
        });
    }

    getData() {
        this.userInfoService.getUserProfile().subscribe(data => {
            this.userData = data as UserProfile;
        });
    }

    getStatistics() {
        this.userInfoService.getStatistics().subscribe(data => {
            this.statisticsInfo = data as Statistics;
            if (this.statisticsInfo.latestUserBoards.length === 0) {
                this.lastBoardDeleted.emit();
            }
        });
    }

    setBorder(element) {
        let borderWidth = 3 * (Math.log2(element));
        if (borderWidth > 85) {
            borderWidth = 85;
        } else if (borderWidth === 0) {
            borderWidth = 3;
        }
        return borderWidth;
    }
}
