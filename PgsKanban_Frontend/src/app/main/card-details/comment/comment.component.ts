import { CommentCardData } from './../../../models/requests/board/commentCardData';
import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'app-comment',
    templateUrl: './comment.component.html',
    styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit {

    @Input() comment: CommentCardData;
    gravatarUrl: string;

    ngOnInit() {
        if (!this.comment.user) {
            this.comment.user = this.comment.externalUser;
        }
        this.gravatarUrl = `https://www.gravatar.com/avatar/${this.comment.user.hashMail}?s=40`;
    }
}
