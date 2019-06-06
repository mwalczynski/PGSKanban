import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';

import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    templateUrl: './remove-member-confirmation.component.html',
    styleUrls: ['./remove-member-confirmation.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class RemoveMemberConfirmationComponent {
    constructor(public dialogRef: MatDialogRef<RemoveMemberConfirmationComponent>, @Inject(MAT_DIALOG_DATA) public data) { }

    public delete() {
        const deleteConfirmed = true;
        this.dialogRef.close(deleteConfirmed);
    }

    public cancel() {
        this.dialogRef.close();
    }
}
