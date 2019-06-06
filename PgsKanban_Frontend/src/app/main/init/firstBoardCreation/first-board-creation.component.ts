import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Url } from '../../../shared/constants';

import { FirstBoardData } from '../../../models/requests/board/firstBoardData';

import { FirstBoardCreationService } from './first-board-creation.service';
import { UserInfoService } from '../../../services/user-info.service';
import * as BoardActions from '../../../actions/board.actions';
import { Store } from '@ngrx/store';
import * as fromApp from '../../../reducers/app.reducers';
import { TrelloImportComponent } from '../../trello-import/trello-import.component';
import { MatDialog } from '@angular/material';

@Component({
    selector: 'app-first-board-creation',
    templateUrl: './first-board-creation.component.html',
    styleUrls: ['./first-board-creation.component.scss']
})
export class FirstBoardCreationComponent implements OnInit {
    initBoard: FirstBoardData = new FirstBoardData();
    isButtonDisabled = false;
    username: string;
    lastname: string;

    constructor(private firstBoardCreationService: FirstBoardCreationService,
                private router: Router,
                private dialog: MatDialog,
                private store: Store<fromApp.AppState>,
                private userInfoService: UserInfoService) {
    }

    ngOnInit() {
        this.getData();
    }

    onCreatingFirstBoardSubmit(form) {
        if (form.invalid) {
            return false;
        }
        this.isButtonDisabled = true;
        this.firstBoardCreationService.createInitBoard(this.initBoard).subscribe(res => {
            this.isButtonDisabled = false;
            const board = res.board;
            this.store.dispatch(new BoardActions.AddBoard(res));
            this.router.navigate([Url.Board, board.obfuscatedId, board.name]);
        }, () => {
            this.isButtonDisabled = false;
        });
    }

    isEmptyCardName() {
        return this.initBoard.cardName === undefined || this.initBoard.cardName === '';
    }

    getData() {
        this.userInfoService.getUserProfile().subscribe((user) => {
            this.username = user.firstName;
            this.lastname = user.lastName;
        });
    }

    showBoardImport() {
        this.dialog.open(TrelloImportComponent);
    }
}
