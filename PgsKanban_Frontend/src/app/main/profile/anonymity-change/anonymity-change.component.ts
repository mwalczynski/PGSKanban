import { Component, OnInit, Input } from '@angular/core';
import { UserInfoService } from '../../../services/user-info.service';

@Component({
    selector: 'app-anonymity-change',
    templateUrl: './anonymity-change.component.html',
    styleUrls: ['./anonymity-change.component.scss']
})
export class AnonymityChangeComponent implements OnInit {
    @Input() isProfileAnonymous: boolean;
    waitingForResponse = false;
    constructor(private userInfoService: UserInfoService) { }

    ngOnInit() {
    }

    onAnonymityChange() {
        this.userInfoService.changeUserAnonymity(this.isProfileAnonymous).subscribe(data => {
            this.waitingForResponse = false;
        });
        this.waitingForResponse = true;
    }
}
