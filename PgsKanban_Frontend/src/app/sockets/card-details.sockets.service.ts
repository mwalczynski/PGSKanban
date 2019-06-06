import { Injectable } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { HubUrlService } from './hub-url.service';
import { Store } from '@ngrx/store';
import * as fromApp from '../reducers/app.reducers';
import * as CardDetailsActions from '../actions/card-details.actions';
import { Comment } from '../models/responses/board/comment';

const CardDetailsHubActions = {
    JoinGroup: 'joinGroup',
    LeaveGroup: 'leaveGroup',
    ChangedLongDescription: 'changedLongDescription',
    ChangedName: 'changedName',
    AddComment: 'addComment'
};

@Injectable()
export class CardDetailsSocketsService {
    hubConnection: HubConnection;
    userEmail: string;

    constructor(private urlService: HubUrlService, private store: Store<fromApp.AppState>) {
        this.store.select('userInfo').subscribe((state) => {
            this.userEmail = state.userProfile.email;
        });
    }

    init(cardId: number) {
        this.hubConnection = new HubConnection(this.urlService.createUrl('details'));
        this.hubConnection.start().then(() => {
            this.hubConnection.invoke(CardDetailsHubActions.JoinGroup, cardId);
        });

        this.hubConnection.on(CardDetailsHubActions.ChangedLongDescription, (newDescription: string) => {
            this.store.dispatch(new CardDetailsActions.ChangeLongDescription(newDescription));
        });

        this.hubConnection.on(CardDetailsHubActions.ChangedName, (newName: string) => {
            this.store.dispatch(new CardDetailsActions.ChangeName(newName));
        });

        this.hubConnection.on(CardDetailsHubActions.AddComment, (comment: Comment) => {
            comment.isOwner = this.userEmail === comment.user.email;
            this.store.dispatch(new CardDetailsActions.AddComment(comment));
        });
    }

    close(cardId: number) {
        this.hubConnection.invoke(CardDetailsHubActions.LeaveGroup, cardId).then(() => {
            this.hubConnection.stop();
        });
    }
}
