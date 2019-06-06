import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ErrorData } from '../../models/modalData/errorData';

@Component({
    templateUrl: './error-modal.component.html',
    styleUrls: ['./error-modal.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ErrorModalComponent implements OnInit {
    ngOnInit(): void {
        if (!this.data) {
            this.data = ErrorData.default();
        }
    }

    constructor(public dialogRef: MatDialogRef<ErrorModalComponent>, @Inject(MAT_DIALOG_DATA) public data: ErrorData) { }

    closeDialog() {
        this.dialogRef.close();
    }

    handleButton() {
        if (this.data.handleButton) {
            this.data.handleButton();
        }
        this.closeDialog();
    }
}
