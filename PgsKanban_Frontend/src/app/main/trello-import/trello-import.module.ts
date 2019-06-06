import { NgModule } from '@angular/core';
import { TrelloImportComponent } from './trello-import.component';
import { MaterialModule } from '../../shared/material.module';
import { FileUploadModule } from 'ng2-file-upload';
import { ReactiveFormsModule } from '@angular/forms';
import { ImportService } from './import.service';
import { LoadingModule } from 'ngx-loading';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        MaterialModule,
        FileUploadModule,
        LoadingModule,
        RouterModule,
        ReactiveFormsModule
    ],
    declarations: [
        TrelloImportComponent
    ],
    providers: [
        ImportService
    ],
    entryComponents: [
        TrelloImportComponent
    ]
})
export class TrelloImportModule {
}
