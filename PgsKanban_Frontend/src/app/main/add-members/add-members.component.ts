import {Component, ElementRef, OnDestroy, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {MatDialogRef} from '@angular/material';
import * as fromApp from '../../reducers/app.reducers';
import {Store} from '@ngrx/store';
import {UserInfoService} from '../../services/user-info.service';
import {Member} from '../../models/responses/board/member';
import {Subscription} from 'rxjs/Subscription';
import {Observable} from 'rxjs/Observable';

const MINIMUM_SEARCH_TEXT_LENGTH = 2;

@Component({
    templateUrl: './add-members.component.html',
    styleUrls: ['./add-members.component.scss'],
    encapsulation: ViewEncapsulation.None
})

export class AddMembersComponent implements OnInit, OnDestroy {
    @ViewChild('searchPhrase') searchPhrase: ElementRef;
    notEnoughCharacters: boolean;
    notFoundMembers: boolean;
    searchSub: Subscription;
    boardId: number;
    members: Member[];

    constructor(public dialogRef: MatDialogRef<AddMembersComponent>, private userInfoService: UserInfoService,
                private store: Store<fromApp.AppState>) {
    }

    ngOnInit(): void {
        this.store.select('boardList').subscribe((state) => {
            this.boardId = state.activeBoard.id;
        });
        this.setUpSearching();
    }

    ngOnDestroy() {
        if (this.searchSub) {
            this.searchSub.unsubscribe();
        }
    }

    closeDialog() {
        this.dialogRef.close();
    }

    searchUsers(searchPhrase: string) {
        if (searchPhrase.length > MINIMUM_SEARCH_TEXT_LENGTH) {
            this.searchSub = this.userInfoService.searchUsers(searchPhrase, this.boardId).subscribe(members => {
                this.members = members;
                this.notFoundMembers = this.members.length === 0;
                this.notEnoughCharacters = false;
            }, () => {
                this.closeDialog();
            });
            return;
        }
        this.handleSearchingTooShortPhrase(searchPhrase);
    }

    showNoResultMessage() {
        return this.notFoundMembers;
    }

    showNoEnoughCharactersMessage() {
        return this.notEnoughCharacters;
    }

    private setUpSearching() {
        Observable
            .fromEvent(this.searchPhrase.nativeElement, 'keyup')
            .map((x: any) => x.currentTarget.value)
            .distinctUntilChanged()
            .debounceTime(200).subscribe(text => this.searchUsers(text));
    }

    private handleSearchingTooShortPhrase(searchPhrase: string) {
        this.members = [];
        this.notFoundMembers = false;
        this.notEnoughCharacters = searchPhrase.length !== 0;
    }
}
