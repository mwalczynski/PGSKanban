import {Component, Input, OnInit} from '@angular/core';
import {Member} from '../../../models/responses/board/member';
import {MatDialog} from '@angular/material';
import {RemoveMemberConfirmationComponent} from '../../../modals/remove-member-confirmation/remove-member-confirmation.component';
import {MemberData} from '../../../models/requests/board/memberData';
import {MemberService} from '../../../services/member.service';
import {Store} from '@ngrx/store';
import {Tooltip} from './../../../shared/constants';
import {TooltipService} from './../../../services/tooltip.service';
import * as fromApp from '../../../reducers/app.reducers';
import * as MemberActions from '../../../actions/member.actions';
import * as BoardActions from '../../../actions/board.actions';

@Component({
    selector: 'app-member-miniature',
    templateUrl: './member-miniature.component.html',
    styleUrls: ['./member-miniature.component.scss']
})
export class MemberMiniatureComponent implements OnInit {
    @Input() member: Member;
    @Input() isAllowedToModify: boolean;
    boardId: number;
    showNameTooltip: boolean;
    showEmailTooltip: boolean;
    tooltipDelay = Tooltip.Delay;
    imageSrc;
    hovering = false;

    ngOnInit(): void {
        if (this.member.pictureSrc) {
            this.imageSrc = this.member.pictureSrc;
        } else {
            this.imageSrc = `https://www.gravatar.com/avatar/${this.member.hashMail}?s=40`;
        }
        this.store.select('boardList').subscribe((state) => {
            this.boardId = state.activeBoard.id;
        });
        this.showNameTooltip = this.tooltipService.shouldHideTooltip(
            this.member.firstName + this.member.lastName,
            Tooltip.MemberMiniatureNameLength
        );
        this.showEmailTooltip = this.tooltipService.shouldHideTooltip(this.member.email, Tooltip.MemberMiniatureEmailLength);
    }

    constructor(private tooltipService: TooltipService,
                private dialog: MatDialog,
                private memberService: MemberService,
                private store: Store<fromApp.AppState>) {
    }

    deleteMemberRequest() {
        this.dialog.open(RemoveMemberConfirmationComponent).afterClosed()
            .subscribe(deleteConfirmed => {
                if (deleteConfirmed) {
                    this.deleteMember();
                }
            });
    }

    deleteMember() {
        const deleteMemberData = new MemberData(this.boardId, this.member.id);
        this.memberService.deleteMember(deleteMemberData).subscribe(() => {
            this.store.dispatch(new MemberActions.DeleteMember(this.member.id));
            this.store.dispatch(new BoardActions.DeleteMemberFromBoard(this.boardId));
        });
    }

    onMouseEnter() {
        this.hovering = true;
    }

    onMouseLeave() {
        this.hovering = false;
    }
}
