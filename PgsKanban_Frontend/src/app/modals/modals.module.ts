import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FlexLayoutModule } from '@angular/flex-layout';

import { ErrorModalComponent } from './error-modal/error-modal.component';
import { SuccessModalComponent } from './success-modal/success-modal.component';

import { DeleteConfirmationComponent } from './delete-confirmation/delete-confirmation.component';
import { RemoveMemberConfirmationComponent } from './remove-member-confirmation/remove-member-confirmation.component';
import { UnassignConfirmationComponent } from './unassign-confirmation/unassign-confirmation.component';

@NgModule({
    imports: [
        CommonModule,
        FlexLayoutModule
    ],
    declarations: [
        ErrorModalComponent,
        SuccessModalComponent,
        DeleteConfirmationComponent,
        RemoveMemberConfirmationComponent,
        UnassignConfirmationComponent
    ],
    entryComponents: [
        ErrorModalComponent,
        SuccessModalComponent,
        DeleteConfirmationComponent,
        RemoveMemberConfirmationComponent,
        UnassignConfirmationComponent
    ]
})
export class ModalsModule { }
