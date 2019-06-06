import { Subscription } from 'rxjs/Subscription';
import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material';

import { AddCardData } from '../../models/requests/board/addCardData';
import { Card } from '../../models/responses/board/card';
import { CardsService } from './card.service';
import { DeleteCardData } from '../../models/requests/board/deleteCardData';
import { CardDetailsComponent } from '../card-details/card-details.component';
import { DeleteConfirmationComponent } from '../../modals/delete-confirmation/delete-confirmation.component';
import { DeleteCardConfirmationData } from '../../models/modalData/deleteCardConfirmationData';

import { Store } from '@ngrx/store';
import * as fromApp from '../../reducers/app.reducers';
import * as DragStatusActions from '../../actions/drag-status.actions';
import * as CardDetailsActions from '../../actions/card-details.actions';

@Component({
    selector: 'app-card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.scss']
})
export class CardComponent implements OnInit, OnDestroy {

    boardSubscription: Subscription;
    isTrashDisplayed = false;
    @Input() card: Card;
    @Input() isOwner: boolean;
    @Output() abortAddingCard: EventEmitter<any> = new EventEmitter();
    @Output() addNewCard: EventEmitter<any> = new EventEmitter();
    @Output() deleteChosenCard: EventEmitter<Number> = new EventEmitter();
    pressed = false;
    lastCardName: string;
    clicked: boolean;
    secondClick: boolean;
    boardId: number;
    boardObfuscatedId: string;
    boardName: string;

    constructor(
        private cardsService: CardsService,
        private dialog: MatDialog,
        private store: Store<fromApp.AppState>) {
    }

    ngOnInit(): void {
        this.boardSubscription = this.store.select('boardList').subscribe((state) => {
            if (state.activeBoard) {
                this.boardId = state.activeBoard.id;
                this.boardObfuscatedId = state.activeBoard.obfuscatedId;
                this.boardName = state.activeBoard.name;
            }
        });
    }

    ngOnDestroy() {
        this.boardSubscription.unsubscribe();
    }

    startEditingCard() {
        if (this.isOwner) {
            this.card.editingInProgress = true;
            this.isTrashDisplayed = false;
            this.lastCardName = this.card.name;
            this.store.dispatch(new DragStatusActions.DisableDragging());
        }
    }

    addCard() {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        if (this.pressed === true) {
            return false;
        }
        this.pressed = true;
        const addCardData: AddCardData = {
            name: this.card.name,
            listId: this.card.listId
        };
        this.cardsService.addCard(addCardData).subscribe((response) => {
            this.card.id = response.id;
            this.card.obfuscatedId = response.obfuscatedId;
            this.card.editingInProgress = false;
            this.card.addingInProgress = false;
            this.cardsService.saveLastCardName(this.boardId, '');
            this.addNewCard.emit();
            this.pressed = false;
        });
    }

    abortChanges(deleteLastCardName: string) {
        this.store.dispatch(new DragStatusActions.EnableDragging());
        if (this.card.addingInProgress) {
            if (deleteLastCardName) {
                this.cardsService.saveLastCardName(this.boardId, '');
            }
            this.abortAddingCard.emit();
        }
        if (this.card.editingInProgress) {
            this.abortEditingCard();
        }
    }

    abortEditingCard() {
        this.card.name = this.lastCardName;
        this.card.editingInProgress = false;
    }

    saveCard(form) {
        if (form.invalid) {
            return;
        }
        if (this.card.addingInProgress) {
            this.addCard();
            return;
        }
        if (this.card.editingInProgress) {
            this.editCard();
        }
    }

    editCard() {
        this.cardsService.editCard(this.card).subscribe(() => {
            this.card.editingInProgress = false;
            this.card.addingInProgress = false;
            this.store.dispatch(new DragStatusActions.EnableDragging());
        });
    }

    showTrash() {
        this.isTrashDisplayed = !(this.card.editingInProgress || this.card.addingInProgress);
    }

    hideTrash() {
        this.isTrashDisplayed = false;
    }

    shouldDisplayTrash() {
        return this.isOwner && this.isTrashDisplayed;
    }

    isCardTextDisplayed() {
        return !(this.card.addingInProgress || this.card.editingInProgress);
    }

    isFormDisplayed() {
        return this.card.addingInProgress || this.card.editingInProgress;
    }

    handleDeletingCard() {
        this.store.dispatch(new DragStatusActions.DisableDragging());
        this.dialog.open(DeleteConfirmationComponent, {
            data: new DeleteCardConfirmationData
        }).afterClosed()
            .subscribe(deleteConfirmed => {
                if (deleteConfirmed) {
                    this.deleteCardData();
                }
            });
    }

    deleteCardData() {
        const deleteCardData: DeleteCardData = {
            cardId: this.card.id,
            listId: this.card.listId
        };
        this.cardsService.deleteCard(deleteCardData).subscribe(() => {
            this.deleteChosenCard.emit(this.card.id);
            this.store.dispatch(new DragStatusActions.EnableDragging());
        });
    }

    updateCardName() {
        if (this.card.addingInProgress) {
            this.cardsService.saveLastCardName(this.boardId, this.card.name);
        }
    }

    handleSingleClick() {
        if (this.clicked) {
            this.secondClick = true;
            return;
        }
        this.clicked = true;
        setTimeout(() => {
            if (this.secondClick) {
                this.startEditingCard();
                this.secondClick = false;
                this.clicked = false;
                return;
            }
            this.showDetails();
            this.secondClick = false;
            this.clicked = false;
        }, 200);
    }

    showDetails() {
        if (this.card.editingInProgress === true || this.card.addingInProgress === true) {
            return;
        }
        this.cardsService.getCardDetails(this.card.obfuscatedId).subscribe(data => {
            const cardDetails = {
                id: data.id,
                obfuscatedId: data.obfuscatedId,
                name: data.name,
                listName: data.listName,
                description: data.description,
                comments: data.comments,
            };
            this.store.dispatch(new CardDetailsActions.GetCardDetails(cardDetails));
            this.dialog.open(CardDetailsComponent);
        });
    }
}
