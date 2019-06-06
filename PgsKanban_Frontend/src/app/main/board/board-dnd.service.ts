import {Injectable} from '@angular/core';
import {DragulaService} from 'ng2-dragula';
import {BoardService} from '../../services/board.service';
import * as autoScroll from 'dom-autoscroller';
import * as fromApp from '../../reducers/app.reducers';
import {Store} from '@ngrx/store';
import {ListsService} from '../../services/lists.service';


@Injectable()
export class BoardDndService {
    isDraggingAllowed = true;
    boardId: number;
    isOwner: boolean;

    autoScroll: any;
    cardAutoScroll: any;
    isDragging = false;

    constructor(private dragulaService: DragulaService,
                private store: Store<fromApp.AppState>,
                private boardService: BoardService,
                private listsService: ListsService) {

        this.store.select('dragStatus').subscribe((state) => {
            this.isDraggingAllowed = state.draggingAllowed;
        });
    }

    setUpDnd(boardId, isOwner) {
        this.boardId = boardId;
        this.isOwner = isOwner;

        this.unsubFromDndEvents();
        this.setupAutoScroll();
        setTimeout(() => {
            this.setupCardAutoScroll();
        });
        this.destroyExistingBags();
        this.setUpListDnd();
        this.setUpCardDnd();
        this.subToDndEvents();
    }

    private setupAutoScroll() {
        if (this.autoScroll) {
            this.autoScroll.destroy(true);
            this.autoScroll = null;
        }

        const thisBoard = document.querySelector('.board__row');
        thisBoard.scrollLeft = 0;
        this.autoScroll = autoScroll([
            thisBoard
        ], {
                margin: 200,
                maxSpeed: 30,
                scrollWhenOutside: true,
                autoScroll: () => {
                    return this.autoScroll.down && this.isDragging;
                }
            });
    }

    private setupCardAutoScroll() {
        if (this.cardAutoScroll) {
            this.cardAutoScroll.destroy(true);
            this.cardAutoScroll = null;
        }
        const lists = document.querySelectorAll('.list__container');
        const listArray = Array.from(lists);
        this.cardAutoScroll = autoScroll(
            listArray, {
                margin: 40,
                maxSpeed: 30,
                scrollWhenOutside: false,
                autoScroll: () => {
                    return this.autoScroll.down && this.isDragging;
                }
            });
    }

    private unsubFromDndEvents() {
        this.dragulaService.drop.observers.pop();
        this.dragulaService.drag.observers.pop();
        this.dragulaService.dragend.observers.pop();
    }

    private subToDndEvents() {
        this.dragulaService.dropModel.subscribe((value) => {
            this.onDrop(value);
        });
        this.dragulaService.dragend.subscribe((value) => {
            this.isDragging = false;
        });
        this.dragulaService.drag.subscribe((value) => {
            this.isDragging = true;
        });
    }

    private destroyExistingBags() {
        if (this.dragulaService.find('lists-bag')) {
            this.dragulaService.destroy('lists-bag');
        }
        if (this.dragulaService.find('cards-bag')) {
            this.dragulaService.destroy('cards-bag');
        }
    }

    private setUpListDnd() {
        this.dragulaService.setOptions('lists-bag', {
            direction: 'horizontal',
            moves: (el, container, handle) => {
                return this.checkIfCanBeDragged()
                    && (handle.className.indexOf('listName--color') !== -1
                        || handle.className.indexOf('list__header') !== -1 ||
                        handle.className.indexOf('list__name') !== -1);
            }
        });
    }

    private setUpCardDnd() {
        this.dragulaService.setOptions('cards-bag', {
            removeOnSpill: false,
            revertOnSpill: true,
            moves: (el, container, target) => {
                if (target.classList) {
                    return target.classList.contains('cardDraggable') && this.checkIfCanBeDragged();
                }
                return false;
            }
        });
    }

    private checkIfCanBeDragged() {
        return this.isOwner && this.isDraggingAllowed;
    }

    private onDrop(args) {
        const el = args[1];
        const targetContainer = args[2];

        if (el.className.indexOf('card') !== -1) {
            const sourceContainer = args[3];
            this.onDropCard(el, targetContainer, sourceContainer);
        }

        if (el.className.indexOf('board__list') !== -1) {
            this.onDropList(el, targetContainer);
        }
    }

    private onDropList(listWrapper, board) {
        const newIndex = this.getListNewPosition(listWrapper, board);
        const list = listWrapper.children[0];
        this.boardService.moveList(list.id, this.boardId, newIndex + 1).subscribe();
    }

    private getListNewPosition(listDivWrapper: HTMLElement, boardElem: HTMLElement) {
        let result = -1;
        for (let i = 0; i < boardElem.children.length && result === -1; i++) {
            const currentElem = boardElem.children.item(i);
            if (currentElem === listDivWrapper) {
                result = i;
            }
        }
        return result;
    }

    private onDropCard(el, targetContainer, sourceContainer) {
        const targetListId = +targetContainer.id;
        const sourceListId = +sourceContainer.id;
        const cardId = +el.children[0].id;
        setTimeout(() => {
            const newPosition = this.getCardNewPosition(targetContainer, el);
            this.listsService.moveCard(cardId, sourceListId, targetListId, newPosition + 1).subscribe();
        });
    }

    private getCardNewPosition(targetListElement: HTMLElement, cardElement: HTMLElement) {
        const cards = Array.from(targetListElement.children);
        for (let i = 0; i < cards.length; i++) {
            const currentElem = targetListElement.children[i];
            if (currentElem.children[0].id === cardElement.children[0].id) {
                return i;
            }
        }
    }
}
