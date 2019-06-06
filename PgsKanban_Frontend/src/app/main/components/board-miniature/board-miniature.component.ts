import { Subscription } from 'rxjs/Subscription';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { UserBoard } from '../../../models/responses/board/userboard';
import { Component, OnInit, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { TooltipService } from '../../../services/tooltip.service';
import { Url, Tooltip } from '../../../shared/constants';
import { DeleteConfirmationComponent } from '../../../modals/delete-confirmation/delete-confirmation.component';
import { DeleteBoardConfirmationData } from '../../../models/modalData/deleteBoardConfirmationData';
import { UnassignConfirmationComponent } from '../../../modals/unassign-confirmation/unassign-confirmation.component';
import * as fromApp from '../../../reducers/app.reducers';
import * as BoardActions from '../../../actions/board.actions';
import { BoardService } from '../../../services/board.service';
import * as DragStatusActions from '../../../actions/drag-status.actions';
import * as StatisticsActions from '../../../actions/statistics.actions';
import { Location } from '@angular/common';

@Component({
    selector: 'board-miniature',
    templateUrl: './board-miniature.component.html',
    styleUrls: ['./board-miniature.component.scss']
})
export class BoardMiniatureComponent implements OnInit, OnDestroy {

    @Input() userBoard: UserBoard;
    @Input() isNotSidebar: boolean;
    @Output() boardHearted = new EventEmitter();

    boardSubscription: Subscription;
    activeBoardId: number;
    imageUrl: string;
    hovering: Boolean = false;
    showBoardNameTooltip: boolean;
    showOwnerTooltip: boolean;
    tooltipDelay = Tooltip.Delay;

    constructor(private router: Router,
                private dialog: MatDialog,
                private boardService: BoardService,
                private tooltipService: TooltipService,
                private location: Location,
                private store: Store<fromApp.AppState>) {
    }

    ngOnInit() {
        this.boardSubscription = this.store.select('boardList').subscribe((state) => {
            if (state.activeBoard) {
                this.activeBoardId = state.activeBoard.id;
            }
        });

        if (this.userBoard.board.owner.pictureSrc) {
            this.imageUrl = this.userBoard.board.owner.pictureSrc;
        } else {
            this.imageUrl = `https://www.gravatar.com/avatar/${this.userBoard.board.owner.hashMail}?s=40`;
        }
        this.showOwnerTooltip = this.tooltipService.shouldHideTooltip(
            this.userBoard.board.owner.firstName + this.userBoard.board.owner.lastName,
            Tooltip.BoardMiniatureOwnerLength
        );
        this.showBoardNameTooltip = this.tooltipService.shouldHideTooltip(this.userBoard.board.name, Tooltip.BoardMiniatureNameLength);
    }

    ngOnDestroy() {
        this.boardSubscription.unsubscribe();
    }

    showMembersIfMany() {
        return this.userBoard.board.membersCount > 1;
    }

    deleteBoard() {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        let dialogToShow;
        let dialogData;

        if (this.userBoard.isOwner) {
            dialogToShow = DeleteConfirmationComponent;
            dialogData = new DeleteBoardConfirmationData;
        } else {
            dialogToShow = UnassignConfirmationComponent;
            dialogData = null;
        }

        this.dialog.open(dialogToShow, {
            data: dialogData
        }).afterClosed()
            .subscribe(deleteConfirmed => {
                if (deleteConfirmed) {
                    this.boardService.deleteBoard(this.userBoard.boardId).subscribe(boardId => {
                        this.store.dispatch(new BoardActions.DeleteBoard(boardId));
                        this.store.dispatch(new DragStatusActions.EnableDragging());
                        if (this.activeBoardId === boardId) {
                            this.redirectToHomePage();
                        }
                        if (this.location.path() === '') {
                            this.store.dispatch(new StatisticsActions.StatisticsShouldBeUpdated());
                        }
                    });
                }
            });
    }

    redirectToHomePage() {
        this.router.navigate([Url.Initial]);
    }

    changeFavorites() {
        if (!this.isNotSidebar) {
            this.userBoard.isFavorite = !this.userBoard.isFavorite;
            this.userBoard.lastTimeSetFavorite = new Date();
            if (this.userBoard.isFavorite) {
                this.boardHearted.emit();
            }
            this.boardService.changeFavorite(this.userBoard.board.id);
        }
    }

    onMouseEnter() {
        this.hovering = true;
    }

    onMouseLeave() {
        this.hovering = false;
    }

    showHeart() {
        if (this.userBoard.isFavorite) {
            return true;
        }
        return this.hovering && !this.isNotSidebar;

    }

    showBin() {
        return this.hovering && !this.isNotSidebar;
    }
}
