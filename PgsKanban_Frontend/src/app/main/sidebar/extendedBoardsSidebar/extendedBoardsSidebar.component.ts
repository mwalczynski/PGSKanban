import * as _ from 'lodash';
import * as fromApp from '../../../reducers/app.reducers';
import {
    Component,
    OnInit,
    Input,
    Output,
    EventEmitter,
    ViewChild,
    OnChanges,
    SimpleChanges,
    OnDestroy
} from '@angular/core';
import { State } from '../../../reducers/board.reducers';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs/Subscription';
import { UserBoard } from '../../../models/responses/board/userboard';
import { UserboardScrollingService } from './userboard-scrolling.service';
import { slideInOutAnimation } from '../../../shared/animations';
import { TrelloImportComponent } from '../../trello-import/trello-import.component';
import { MatDialog } from '@angular/material';

@Component({
    selector: 'app-extended-boards-sidebar',
    templateUrl: './extendedBoardsSidebar.component.html',
    styleUrls: ['./extendedBoardsSidebar.component.scss'],
    animations: [slideInOutAnimation]
})

export class ExtendedBoardsSidebarComponent implements OnInit, OnDestroy, OnChanges {
    @Output() foldSidebar: EventEmitter<any> = new EventEmitter();
    @Input() boardHearted: any;
    @Input() isBoardsSidebarShown: boolean;
    isScrollingTop = false;
    disableImportTooltip = false;
    userBoardList: Store<State>;
    userBoards: UserBoard[];
    userBoardsSubscription: Subscription;
    @ViewChild('sidebarContainer') boardContainer;

    constructor(private scrollingService: UserboardScrollingService,
                private dialog: MatDialog,
                private store: Store<fromApp.AppState>) {
        this.userBoardList = this.store.select('boardList');
    }

    foldBoardsSidebar() {
        this.foldSidebar.emit();
    }

    ngOnInit(): void {
        this.userBoardsSubscription = this.userBoardList.subscribe(state => {
            this.userBoards = state.userBoards;
        });
        this.scrollToActiveBoardMiniature();
    }

    ngOnDestroy() {
        this.userBoardsSubscription.unsubscribe();
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.isBoardsSidebarShown) {
            if (changes.isBoardsSidebarShown.currentValue) {
                this.scrollToActiveBoardMiniature();
            }
        }
    }

    isAnyFavouritesBoards() {
        return _.some(this.userBoards, ['isFavorite', true]);
    }

    isAnyOwnedBoards() {
        return _.some(this.userBoards, {
            'isFavorite': false,
            'isOwner': true
        });
    }

    isAnyParticipantBoards() {
        return _.some(this.userBoards, {
            'isFavorite': false,
            'isOwner': false
        });
    }

    scrollToActiveBoardMiniature() {
        setTimeout(() => {
            this.scrollingService.scrollToActiveBoardMiniature(this.boardContainer);
        });
    }

    showBoardImport() {
        this.disableImportTooltip = true;
        this.dialog.open(TrelloImportComponent).afterClosed().subscribe(() => {
            this.disableImportTooltip = false;
        });
    }

    scrollTop() {
        setTimeout(() => {
            this.scrollingService.scrollTop(this.boardContainer);
        }, 10);
        setTimeout(() => {
            this.isScrollingTop = false;
        }, 100);
    }
}
