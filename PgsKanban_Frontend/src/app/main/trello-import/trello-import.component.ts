import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MatStepper } from '@angular/material';
import { FileUploader } from 'ng2-file-upload';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ImportService } from './import.service';
import { ImportStatistics } from '../../models/responses/board/import-statistics';
import { Store } from '@ngrx/store';
import * as BoardActions from '../../actions/board.actions';
import * as fromApp from '../../reducers/app.reducers';
import * as StatisticsActions from '../../actions/statistics.actions';
import { Router } from '@angular/router';
import { ErrorModalComponent } from '../../modals/error-modal/error-modal.component';
import { SPINNER_CONFIG } from '../../shared/constants';

@Component({
    templateUrl: './trello-import.component.html',
    styleUrls: ['./trello-import.component.scss']
})
export class TrelloImportComponent implements OnInit {
    @ViewChild('stepper') stepper: MatStepper;
    uploader: FileUploader;
    error = false;
    waitingForResponse = false;
    firstFormGroup: FormGroup;
    statistics: ImportStatistics;
    secondFormGroup: FormGroup;
    boardName: string;
    file: any;
    reader: FileReader;
    hasBaseDropZoneOver = false;
    finishedFlow = false;

    constructor(private dialogRef: MatDialogRef<TrelloImportComponent>,
                private formBuilder: FormBuilder,
                private store: Store<fromApp.AppState>,
                private router: Router,
                private dialog: MatDialog,
                private importService: ImportService) {
        this.uploader = new FileUploader({});
        this.reader = new FileReader();
    }

    ngOnInit() {
        this.reader.onload = (ev: any) => {
            try {
                const boardName = JSON.parse(ev.target.result).name;
                if (!boardName) {
                    this.handleInvalidFile();
                    return;
                }
                this.handleValidFile(boardName);
            } catch (_) {
                this.handleInvalidFile();
            }
        };

        this.uploader.onAfterAddingFile = fileItem => {
            if (this.uploader.queue.length > 1) {
                this.uploader.removeFromQueue(this.uploader.queue[0]);
            }
            this.file = fileItem._file;
            this.reader.readAsText(this.file);
        };

        this.setUpForms();
    }

    getPlaceholderText() {
        if (this.file) {
            return this.file.name;
        }
        if (this.error) {
            return 'Invalid file. Choose another one';
        }
        return 'Drop file here or click to browse';
    }

    importBoard(boardName: string) {
        this.waitingForResponse = true;
        this.importService.importBoard(this.file, boardName).subscribe((statistics) => {
                this.statistics = statistics;
                statistics.userBoard.isOwner = true;
                this.store.dispatch(new BoardActions.AddBoard(statistics.userBoard));
                this.store.dispatch(new StatisticsActions.StatisticsShouldBeUpdated());
                this.secondFormGroup.controls.firstCtrl.setValue(true);
                this.finishedFlow = true;
                this.waitingForResponse = false;
                this.stepper.next();
            },
            () => {
                this.waitingForResponse = false;
                this.dialogRef.close();
                this.dialog.open(ErrorModalComponent);
            });
    }

    fileOverBase(event) {
        this.hasBaseDropZoneOver = event;
    }

    close() {
        this.dialogRef.close();
    }

    getSpinnerConfig() {
        return SPINNER_CONFIG;
    }

    private handleValidFile(boardName: any) {
        this.error = false;
        this.boardName = boardName;
        this.firstFormGroup.controls.firstCtrl.setValue(true);
        this.secondFormGroup.controls.boardName.setValue(this.boardName);
        this.stepper.next();
    }

    private handleInvalidFile() {
        this.error = true;
        this.firstFormGroup.controls.firstCtrl.setValue(false);
        this.boardName = null;
        this.uploader.clearQueue();
        this.file = null;
    }

    private setUpForms() {
        this.firstFormGroup = this.formBuilder.group({
            firstCtrl: ['', Validators.requiredTrue]
        });

        this.secondFormGroup = this.formBuilder.group({
            firstCtrl: ['', Validators.requiredTrue],
            boardName: ['', Validators.required]
        });
    }
}
