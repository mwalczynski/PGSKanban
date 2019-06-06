import { CardDetailsComponent } from './main/card-details/card-details.component';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { MaterialModule } from './shared/material.module';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthModule } from './auth/auth.module';
import { MainModule } from './main/main.module';
import { AuthRequestInterceptor } from './interceptors/auth-request.interceptor';
import { ErrorResponseInterceptor } from './interceptors/error-response.interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DirectivesModule } from './directives/directives.module';
import { RecoverService } from './auth/recover-password/recover.service';
import { ErrorsModule } from './errors/errors.module';
import { ServicesModule } from './services/services.module';
import { DragulaModule } from 'ng2-dragula';
import { HttpModule } from '@angular/http';
import { StoreModule } from '@ngrx/store';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { reducers } from './reducers/app.reducers';
import { MarkdownModule } from 'angular2-markdown';
import { environment } from '../environments/environment';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        DirectivesModule,
        MaterialModule,
        FlexLayoutModule,
        BrowserModule,
        RouterModule,
        MainModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        AuthModule,
        HttpClientModule,
        ErrorsModule,
        DragulaModule,
        HttpModule,
        ServicesModule,
        StoreModule.forRoot(reducers),
        MarkdownModule.forRoot(),
        StoreRouterConnectingModule,
        environment.production ? [] : StoreDevtoolsModule.instrument()
],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthRequestInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorResponseInterceptor,
            multi: true,
        },
        RecoverService
    ],
    entryComponents: [
        CardDetailsComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
