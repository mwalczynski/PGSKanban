import { BoardDndService } from './../board/board-dnd.service';
import { Url } from './../../shared/constants';
import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BoardService } from '../../services/board.service';
import { SharedService } from '../shared.service';
import { ExternalLoginService } from '../../services/external-login.service';
import { Member } from '../../models/responses/board/member';
import * as fromApp from '../../reducers/app.reducers';
import * as MemberActions from '../../actions/member.actions';
import { Store } from '@ngrx/store';
import 'rxjs/add/operator/filter';
import { Board } from '../../models/responses/board/board';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})

export class SidebarComponent implements OnInit {
    @Input() isMembersSidebarActive: boolean;
    @Input() isBoardsSidebarActive: boolean;
    isAllowedToModify: boolean;
    isMouseOverSidebar: boolean;
    activeBoardId: number;
    members: Member[];

    constructor(private externalLoginService: ExternalLoginService,
        private router: Router,
        private boardsService: BoardService,
        private sharedService: SharedService,
        private boardDnd: BoardDndService,
        private store: Store<fromApp.AppState>) {
    }

    ngOnInit(): void {
        this.sharedService.onSidebarsFold.subscribe(() => {
            this.isBoardsSidebarActive = false;
            this.isMembersSidebarActive = false;
        });
        this.store.select('boardList').subscribe((state) => {
            if (state.activeBoard && this.activeBoardId !== state.activeBoard.id) {
                this.activeBoardId = state.activeBoard.id;
                this.getBoardMembers(this.activeBoardId);
            }
        });
    }

    logout() {
        this.externalLoginService.logout();
    }

    redirectToInitPage() {
        this.isMembersSidebarActive = false;
        if (this.router.url !== Url.Initial) {
            this.router.navigate([Url.Initial]);
        }
    }

    foldBoardsSidebar() {
        this.isBoardsSidebarActive = false;
    }

    foldMembersSidebar() {
        this.isMembersSidebarActive = false;
    }

    toggleBoardsSidebar() {
        let delay = 0;
        if (this.isMembersSidebarActive) {
            this.isMembersSidebarActive = false;
            delay = 100;
        }
        setTimeout(() => {
            this.isBoardsSidebarActive = !this.isBoardsSidebarActive;
        }, delay);
    }

    toggleMembersSidebar() {
        if (this.isInBoardView()) {
            let delay = 0;
            if (this.isBoardsSidebarActive) {
                this.isBoardsSidebarActive = false;
                delay = 100;
            }
            setTimeout(() => {
                this.isMembersSidebarActive = !this.isMembersSidebarActive;
            }, delay);
        }
    }

    getBoardMembers(boardId) {
        this.boardsService.getMembers(boardId).subscribe((members) => {
            this.store.dispatch(new MemberActions.GetMembers(members.members));
            this.isAllowedToModify = members.isAllowedToModify;
        });
    }

    isInBoardView() {
        return this.router.url.indexOf(`${Url.Board}/`) !== -1;
    }

    mouseOverSidebar() {
        this.isMouseOverSidebar = true;
    }

    mouseOutOfSidebar() {
        this.isMouseOverSidebar = false;
    }

    sidebarHovering() {
        if (this.isMouseOverSidebar && this.boardDnd.isDragging) {
            return true;
        }
    }
}
