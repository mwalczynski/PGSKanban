import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { SuccessData } from '../../models/modalData/successData';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    templateUrl: './success-modal.component.html',
    styleUrls: ['./success-modal.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class SuccessModalComponent implements OnInit {

    ngOnInit(): void {
        if (!this.data) {
            this.data = SuccessData.default();
        }
    }
    constructor(public dialogRef: MatDialogRef<SuccessModalComponent>, @Inject(MAT_DIALOG_DATA) public data: SuccessData) { }

    closeDialog() {
        this.dialogRef.close();
    }
}
