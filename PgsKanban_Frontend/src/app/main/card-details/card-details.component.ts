import { Subscription } from 'rxjs/Subscription';
import { Router } from '@angular/router';
import { ACTIVATE_SOCKETS, Url } from '../../shared/constants';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import * as fromApp from '../../reducers/app.reducers';
import { Tooltip } from '../../shared/constants';
import { TooltipService } from '../../services/tooltip.service';
import { UserProfile } from '../../models/responses/profile/userProfile';
import { CommentCardData } from '../../models/requests/board/commentCardData';
import { CardDetailsService } from './card-details.service';
import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { CardDetails } from '../../models/responses/board/cardDetails';
import { Comment } from '../../models/responses/board/comment';
import { CardDetailsSocketsService } from '../../sockets/card-details.sockets.service';
import { NormalizeNameService } from '../../services/normalize-name.service';
import * as CardDetailsActions from '../../actions/card-details.actions';

@Component({
    selector: 'app-card-details',
    templateUrl: './card-details.component.html',
    styleUrls: ['./card-details.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class CardDetailsComponent implements OnInit, OnDestroy {

    cardDetailsSubscription: Subscription;
    activeBoardSubscription: Subscription;
    userInfoSubscription: Subscription;
    showListNameTooltip: boolean;
    showCardNameTooltip: boolean;
    editingCardDescription: string;
    isEditing = false;
    isInfoDisplayed = false;
    formattingClicked = false;
    profile: UserProfile;
    tooltipDelay = Tooltip.Delay;
    cardDetail: CardDetails;
    comment: string;
    comments: Comment[];

    constructor(private dialogRef: MatDialogRef<CardDetailsComponent>,
                private location: Location,
                private router: Router,
                private store: Store<fromApp.AppState>,
                private cardDetailsService: CardDetailsService,
                private cardDetailsSocketService: CardDetailsSocketsService,
                private tooltipService: TooltipService,
                private normalizeNameService: NormalizeNameService) {
    }

    ngOnInit() {
        this.cardDetailsSubscription = this.store.select('cardDetails').subscribe(state => {
            this.cardDetail = state.cardDetails;
            if (!this.cardDetail.description) {
                this.cardDetail.description = '';
            }
            this.comments = this.cardDetail.comments;
            this.location.go(this.router.createUrlTree([Url.CardDetails, this.cardDetail.obfuscatedId, this.cardDetail.name]).toString());
        });

        this.activeBoardSubscription = this.store.select('boardList').subscribe(state => {
            if (this.cardDetail) {
                this.cardDetail.boardName = state.activeBoard.name;
                this.cardDetail.boardId = state.activeBoard.obfuscatedId;
                this.cardDetail.isOwner = state.activeBoard.isOwner;
            }
        });

        this.userInfoSubscription = this.store.select('userInfo').subscribe(state => {
            this.profile = state.userProfile;
        });

        const normalizedCardName = this.normalizeNameService.normalizeName(this.cardDetail.name);
        this.location.go(this.router.createUrlTree([Url.CardDetails, this.cardDetail.obfuscatedId, normalizedCardName]).toString());

        this.showListNameTooltip = this.tooltipService.shouldHideTooltip(this.cardDetail.listName, Tooltip.CardDetailsListNameLength);
        this.showCardNameTooltip = this.tooltipService.shouldHideTooltip(this.cardDetail.name, Tooltip.CardDetailsCardNameLength);

        this.dialogRef.updatePosition({top: '100px'});

        if (ACTIVATE_SOCKETS) {
            this.cardDetailsSocketService.init(this.cardDetail.id);
        }
    }

    ngOnDestroy() {
        const normalizedBoardName = this.normalizeNameService.normalizeName(this.cardDetail.boardName);
        this.location.go(this.router.createUrlTree([Url.Board, this.cardDetail.boardId, normalizedBoardName]).toString());
        this.cardDetailsSubscription.unsubscribe();
        this.activeBoardSubscription.unsubscribe();
        this.userInfoSubscription.unsubscribe();
        if (ACTIVATE_SOCKETS) {
            this.cardDetailsSocketService.close(this.cardDetail.id);
        }
    }

    close() {
        this.dialogRef.close();
    }

    startEditing() {
        if (!ACTIVATE_SOCKETS && this.cardDetail.isOwner) {
            this.editingCardDescription = this.cardDetail.description;
            this.isEditing = true;
        }
    }

    submitEditing() {
        if (this.cardDetail.description === this.editingCardDescription) {
            this.isEditing = false;
            return;
        }

        const editCardDescriptionData = {
            id: this.cardDetail.id,
            description: this.deleteWhitespaces(this.editingCardDescription)
        };

        this.cardDetailsService.editCardDescription(editCardDescriptionData).subscribe(() => {
            this.isEditing = false;
            if (!ACTIVATE_SOCKETS) {
                this.cardDetail.description = editCardDescriptionData.description;
            }
        }, () => {
            this.close();
        });
    }

    toggleInfo() {
        this.isInfoDisplayed = !this.isInfoDisplayed;
        this.formattingClicked = true;
    }

    closeInfo() {
        if (this.isInfoDisplayed) {
            if (!this.formattingClicked) {
                this.isInfoDisplayed = false;
            } else {
                this.formattingClicked = false;
            }
        }
    }

    submitCommenting() {
        if (!this.comment || (/^\s*$/.test(this.comment))) {
            return this.comment = '';
        }

        const addCommentData: CommentCardData = {
            cardId: this.cardDetail.id,
            content: this.comment,
            isOwner: this.cardDetail.isOwner,
            user: this.profile
        };

        this.cardDetailsService.addComment(addCommentData).subscribe(() => {
            if (!ACTIVATE_SOCKETS) {
                this.store.dispatch(new CardDetailsActions.AddComment({
                    content: addCommentData.content,
                    isOwner: true,
                    cardId: addCommentData.cardId,
                    user: this.profile
                }));
            }
        }, () => {
            this.close();
        });
        this.comment = '';
    }

    private deleteWhitespaces(value: string): string {
        return value.replace(/ +/g, ' ').replace(/\n\s*\n/g, '\n');
    }
}
