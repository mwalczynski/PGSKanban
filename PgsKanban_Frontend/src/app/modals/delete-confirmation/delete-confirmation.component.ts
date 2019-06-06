import { ConfirmationData } from './../../models/modalData/confirmationData';
import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';

import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    templateUrl: './delete-confirmation.component.html',
    styleUrls: ['./delete-confirmation.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class DeleteConfirmationComponent implements OnInit {
    message: String;

    constructor(public dialogRef: MatDialogRef<DeleteConfirmationComponent>, @Inject(MAT_DIALOG_DATA) public data) { }

    ngOnInit(): void {
        this.message = this.data.message;
    }

    public delete() {
        const deleteConfirmed = true;
        this.dialogRef.close(deleteConfirmed);
    }

    public cancel() {
        this.dialogRef.close();
    }
}
