import { List } from '../../models/responses/board/list';
import { ListsService } from '../../services/lists.service';
import { AddListData } from '../../models/requests/board/addListData';
import { MatDialog } from '@angular/material';
import { CardsService } from '../card/card.service';
import { DeleteListData } from '../../models/requests/board/deleteListData';
import { DeleteConfirmationComponent } from '../../modals/delete-confirmation/delete-confirmation.component';
import { DeleteListConfirmationData } from '../../models/modalData/deleteListConfirmationData';

import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import * as fromApp from '../../reducers/app.reducers';
import { Store } from '@ngrx/store';
import * as DragStatusActions from '../../actions/drag-status.actions';
import { Card } from '../../models/responses/board/card';
import { Tooltip } from './../../shared/constants';
import { BoardDndService } from './../board/board-dnd.service';
import { DragulaService } from 'ng2-dragula';
import { TooltipService } from './../../services/tooltip.service';

@Component({
    selector: 'app-list',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

    @Input() list: List;
    @Input() isOwner: boolean;
    @Output() deleteChosenList: EventEmitter<number> = new EventEmitter();
    @Output() abortAddingList: EventEmitter<any> = new EventEmitter();
    @Output() listAdded: EventEmitter<any> = new EventEmitter();
    @Output() isAllowedDragging: EventEmitter<boolean> = new EventEmitter();
    hoveringOverListHeader: boolean;
    triedToAddCard: boolean;
    lastValueOfName: string;
    hoveringOverAddCardButton: boolean;
    showTooltip: boolean;
    tooltipDelay = Tooltip.Delay;
    cards: Card[];
    isMouseOverButton: boolean;

    constructor(private listsService: ListsService,
        public dialog: MatDialog,
        private cardsService: CardsService,
        private boardDnd: BoardDndService,
        private store: Store<fromApp.AppState>,
        public tooltipService: TooltipService) {
    }

    ngOnInit() {
        this.cards = this.list.cards.sort((card1, card2) => card1.position - card2.position);

        if (!this.list.addingInProgress) {
            this.showTooltip = this.tooltipService.shouldHideTooltip(this.list.name, Tooltip.ListNameLength);
        }
    }

    addCard() {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        this.list.cards.push({
            name: this.cardsService.getLastCardName(this.list.boardId),
            addingInProgress: true,
            listId: this.list.id
        });
    }

    saveCard() {
        if (this.list.cards.some(this.checkIfAddingMode)) {
            if (this.list.addingInProgress) {
                if (this.list.name === '' || this.list.name === undefined || this.list.name === null) {
                    this.triedToAddCard = true;
                } else {
                    this.addList(false, this.handleAddingCard);
                }
            } else {
                this.handleAddingCard();
            }
        } else {
            this.addCard();
        }
    }

    handleAddingCard = () => {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        if (this.cards.some(this.checkIfAddingMode)) {
            const card = this.cards.find(this.checkIfAddingMode);
            if (card.name === '' || card.name === undefined || card.name === null) {
                return;
            }
            this.cardsService.addCard(card).subscribe((response) => {
                card.id = response.id;
                card.obfuscatedId = response.obfuscatedId;
                card.editingInProgress = false;
                card.addingInProgress = false;
                this.cardsService.saveLastCardName(this.list.boardId, '');
                this.addCard();
            });
        } else {
            this.addCard();
        }
    }

    deleteLastCard() {
        this.cards.pop();
    }

    deleteCard(cardId) {
        this.cards = this.cards.filter(x => x.id !== cardId);
    }

    checkIfAddingMode(element) {
        return element.addingInProgress;
    }

    mouseLeaveHeader() {
        this.hoveringOverListHeader = false;
    }

    mouseOverHeader() {
        if (this.isOwner) {
            this.hoveringOverListHeader = true;
        }
    }

    checkHoveringHeader() {
        return this.hoveringOverListHeader &&
            !this.list.isEditing &&
            !this.boardDnd.isDragging &&
            !this.list.addingInProgress ? 'hovering' : '';
    }

    isTextDisplayed() {
        const condition = !(this.list.addingInProgress || this.list.isEditing);
        return condition;
    }

    isFormDisplayed() {
        return this.list.isEditing || this.list.addingInProgress;
    }

    startEditing() {
        if (this.isOwner) {
            this.list.isEditing = true;
            this.lastValueOfName = this.list.name;
            this.store.dispatch(new DragStatusActions.DisableDragging());
        }
    }

    addList(emitAdding: boolean, callback?: () => void) {
        const addListData: AddListData = {
            name: this.list.name,
            boardId: this.list.boardId
        };
        return this.listsService.addList(addListData).subscribe((response) => {
            this.list.id = response.id;
            this.list.isEditing = false;
            this.list.addingInProgress = false;
            this.listsService.saveLastListName(this.list.boardId, '');
            if (emitAdding) {
                this.listAdded.emit();
            }
            if (callback) {
                callback();
            }
            this.showTooltip = this.tooltipService.shouldHideTooltip(this.list.name, Tooltip.ListNameLength);
        });
    }

    saveList(form) {
        if (form.invalid) {
            return;
        }
        this.store.dispatch(new DragStatusActions.EnableDragging());
        if (this.list.addingInProgress) {
            this.addList(true);
            return;
        }
        if (this.list.isEditing) {
            this.editList();
        }
    }

    editList() {
        this.listsService.editList(this.list).subscribe(() => {
            this.list.isEditing = false;
            this.list.addingInProgress = false;
            this.store.dispatch(new DragStatusActions.EnableDragging());
            this.showTooltip = this.tooltipService.shouldHideTooltip(this.list.name, Tooltip.ListNameLength);
        });
    }

    deleteListRequest() {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        this.dialog.open(DeleteConfirmationComponent, {
            data: new DeleteListConfirmationData
        }).afterClosed()
            .subscribe(deleteConfirmed => {
                if (deleteConfirmed) {
                    this.deleteList();
                }
            });
    }

    deleteList() {
        const deleteListData: DeleteListData = {
            listId: this.list.id,
            boardId: this.list.boardId
        };
        this.listsService.deleteList(deleteListData).subscribe(() => {
            this.deleteChosenList.emit(this.list.id);
            this.store.dispatch(new DragStatusActions.EnableDragging());
        });
    }

    abortChanges(deleteLastListName: string) {
        if (this.list.addingInProgress) {
            if (deleteLastListName) {
                this.listsService.saveLastListName(this.list.boardId, '');
            }
            this.abortAddingList.emit();
            this.store.dispatch(new DragStatusActions.EnableDragging());
        }
        if (this.list.isEditing) {
            this.abortEditing();
        }
    }

    abortEditing() {
        this.list.name = this.lastValueOfName;
        this.list.isEditing = false;
        this.store.dispatch(new DragStatusActions.EnableDragging());
    }

    updateListName() {
        if (this.list.addingInProgress) {
            this.listsService.saveLastListName(this.list.boardId, this.list.name);
        }
    }

    isErrorDisplayed(form) {
        return form.invalid && (form.dirty || this.triedToAddCard);
    }

    checkIfOwner() {
        return this.isOwner;
    }

    mouseOverButton() {
        return this.isMouseOverButton = true;
    }

    mouseOutOfButton() {
        return this.isMouseOverButton = false;
    }

    isDragging() {
        if (this.isMouseOverButton) {
            return this.boardDnd.isDragging;
        }
        return true;
    }
}
