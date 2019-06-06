import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';

import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    templateUrl: './unassign-confirmation.component.html',
    styleUrls: ['./unassign-confirmation.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class UnassignConfirmationComponent {
    constructor(public dialogRef: MatDialogRef<UnassignConfirmationComponent>, @Inject(MAT_DIALOG_DATA) public data) { }

    public delete() {
        const deleteConfirmed = true;
        this.dialogRef.close(deleteConfirmed);
    }

    public cancel() {
        this.dialogRef.close();
    }
}
