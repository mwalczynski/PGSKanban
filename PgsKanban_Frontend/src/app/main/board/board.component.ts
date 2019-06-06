import { CardDetailsComponent } from '../card-details/card-details.component';
import { MatDialog } from '@angular/material';
import { List } from '../../models/responses/board/list';
import { ListsService } from '../../services/lists.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Board } from '../../models/responses/board/board';
import { BoardService } from '../../services/board.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Url, Tooltip } from '../../shared/constants';
import { BoardDndService } from './board-dnd.service';
import { TooltipService } from './../../services/tooltip.service';
import * as BoardActions from '../../actions/board.actions';
import * as fromApp from '../../reducers/app.reducers';
import { SharedService } from '../shared.service';
import { Store } from '@ngrx/store';
import * as DragStatusActions from '../../actions/drag-status.actions';
import * as CardDetailsActions from '../../actions/card-details.actions';
import { CardDetails } from '../../models/responses/board/cardDetails';
import { NormalizeNameService } from '../../services/normalize-name.service';

@Component({
    selector: 'app-board',
    templateUrl: './board.component.html',
    styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit {
    board: Board;
    lastValue: string;
    isOwner: boolean;
    lists: List[];
    showTooltip: boolean;
    tooltipDelay = Tooltip.Delay;
    isMouseOverButton: boolean;

    constructor(private listsService: ListsService,
                private boardService: BoardService,
                private location: Location,
                private router: Router,
                private route: ActivatedRoute,
                private boardDnd: BoardDndService,
                private sharedService: SharedService,
                private store: Store<fromApp.AppState>,
                private dialog: MatDialog,
                private tooltipService: TooltipService,
                private normalizeNameService: NormalizeNameService) {
    }

    ngOnInit() {
        this.route.data.subscribe(val => {
            this.board = val['board'];
            this.isOwner = this.board.isOwner;
            const cardDetails = val['cardDetails'];
            if (cardDetails) {
                this.showCardDetails(cardDetails);
            }
            this.getLists();
            this.editPath();
            this.boardDnd.setUpDnd(this.board.id, this.isOwner);
            this.store.dispatch(new BoardActions.EditActiveBoard(this.board));
        });
        this.showTooltip = this.tooltipService.shouldHideTooltip(this.board.name, Tooltip.BoardNameLength);
    }

    getLists() {
        this.lists = this.board.lists.sort((list1, list2) => list1.position - list2.position);
    }

    deleteList(listId) {
        this.lists = this.lists.filter(x => x.id !== listId);
    }

    saveList() {
        const list = this.lists.find(x => x.addingInProgress);
        if (list) {
            if (list.name === '' || list.name === undefined || list.name === null) {
                return;
            }
            this.listsService.addList(list).subscribe((response) => {
                list.id = response.id;
                list.isEditing = false;
                list.addingInProgress = false;
                this.listsService.saveLastListName(this.board.id, '');
                this.addList();
            });
        } else {
            this.addList();
        }
    }

    addList() {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        this.lists.push({
            name: this.listsService.getLastListName(this.board.id),
            addingInProgress: true,
            boardId: this.board.id,
            cards: []
        });
    }

    deleteLastList() {
        this.lists.pop();
    }

    saveBoard(form) {
        if (form.$invalid) {
            return false;
        }

        if (this.board.isEditing) {
            this.editBoard();
        }
    }

    editBoard() {
        if (this.board.name !== this.lastValue) {
            this.store.dispatch(new DragStatusActions.EnableDragging());
            this.store.dispatch(new BoardActions.EditBoard(this.board));
            this.boardService.editBoard(this.board).subscribe(() => {
                this.board.isEditing = false;
                this.editPath();
                this.showTooltip = this.tooltipService.shouldHideTooltip(this.board.name, Tooltip.BoardNameLength);
            });
        }
    }

    editPath() {
        const normalizedName = this.normalizeNameService.normalizeName(this.board.name);
        this.location.go(this.router.createUrlTree([Url.Board, this.board.obfuscatedId, normalizedName]).toString());
    }

    abortEditing() {
        this.board.name = this.lastValue;
        this.board.isEditing = false;
        this.store.dispatch(new DragStatusActions.EnableDragging());
    }

    startEditing() {
        if (this.isOwner) {
            this.store.dispatch(new DragStatusActions.DisableDragging());
            this.lastValue = this.board.name;
            this.board.isEditing = true;
        }
    }

    isTextDisplayed() {
        return !this.board.isEditing;
    }

    isFormDisplayed() {
        return this.board.isEditing;
    }

    isErrorDisplayed(form) {
        return form.invalid;
    }

    onClickOutsideSidebar() {
        this.sharedService.foldSidebars();
    }

    showCardDetails(cardDetails: CardDetails) {
        this.store.dispatch(new CardDetailsActions.GetCardDetails(cardDetails));
        this.dialog.open(CardDetailsComponent);
    }

    mouseOverButton() {
        return this.isMouseOverButton = true;
    }

    mouseOutOfButton() {
        return this.isMouseOverButton = false;
    }

    isDragging() {
        if (this.isMouseOverButton) {
            if (this.boardDnd.isDragging) {
                return true;
            } else {
                return false;
            }
        }
        return true;
    }
}
