import {Component, Input, EventEmitter, Output} from '@angular/core';
import {slideInOutAnimation} from '../../../shared/animations';
import {Member} from '../../../models/responses/board/member';
import * as fromApp from '../../../reducers/app.reducers';
import {State} from '../../../reducers/member.reducers';
import {Store} from '@ngrx/store';
import {MatDialog} from '@angular/material';
import {AddMembersComponent} from '../../add-members/add-members.component';

@Component({
    selector: 'app-extended-members-sidebar',
    templateUrl: './extendedMembersSidebar.component.html',
    styleUrls: ['./extendedMembersSidebar.component.scss'],
    animations: [slideInOutAnimation]
})
export class ExtendedMembersSidebarComponent {
    @Output() foldSidebar: EventEmitter<any> = new EventEmitter();
    @Input() isMembersSidebarShown: boolean;
    @Input() members: Member[];
    @Input() isAllowedToModify: boolean;
    memberList: Store<State>;

    constructor(private store: Store<fromApp.AppState>, private dialog: MatDialog) {
        this.memberList = this.store.select('memberList');
    }

    showMemberAddingDialog() {
        this.dialog.open(AddMembersComponent);
    }

    foldMembersSidebar() {
        this.foldSidebar.emit();
    }
}
