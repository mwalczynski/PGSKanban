import {Component, Input, OnInit, Output, EventEmitter} from '@angular/core';
import {Member} from '../../../models/responses/board/member';
import {Store} from '@ngrx/store';
import * as fromApp from '../../../reducers/app.reducers';
import * as BoardActions from '../../../actions/board.actions';
import * as MemberActions from '../../../actions/member.actions';
import {MemberData} from '../../../models/requests/board/memberData';
import {MemberService} from '../../../services/member.service';

@Component({
    selector: 'app-member-to-add',
    templateUrl: './member-to-add.component.html',
    styleUrls: ['./member-to-add.component.scss']
})
export class MemberToAddComponent implements OnInit {
    @Input() member: Member;
    @Output() handleError = new EventEmitter<any>();
    imageSrc: string;
    boardId: number;
    waitingForResponse = false;

    constructor(private store: Store<fromApp.AppState>, private memberService: MemberService) {
    }

    ngOnInit() {
        this.store.select('boardList').subscribe((state) => {
            this.boardId = state.activeBoard.id;
        });
        if (this.member.pictureSrc) {
            this.imageSrc = this.member.pictureSrc;
        } else {
            this.imageSrc = `https://www.gravatar.com/avatar/${this.member.hashMail}?s=40`;
        }
    }

    handleMember() {
        if (this.waitingForResponse) {
            return;
        }
        const memberData = new MemberData(this.boardId, this.member.id);
        if (this.member.added) {
            this.deleteMember(memberData);
        } else {
            this.addMember(memberData);
        }
    }

    addMember(memberData: MemberData) {
        this.waitingForResponse = true;
        this.member.added = true;
        this.memberService.addMemberToBoard(memberData)
            .subscribe((addedMember: Member) => {
                this.waitingForResponse = false;
                this.store.dispatch(new BoardActions.AddMemberToBoard(this.boardId));
                this.store.dispatch(new MemberActions.AddMember(addedMember));
            });
    }

    deleteMember(memberData: MemberData) {
        this.waitingForResponse = true;
        this.member.added = false;
        this.memberService.deleteMember(memberData).subscribe(() => {
            this.waitingForResponse = false;
            this.store.dispatch(new BoardActions.DeleteMemberFromBoard(this.boardId));
            this.store.dispatch(new MemberActions.DeleteMember(this.member.id));
        });
    }
}
