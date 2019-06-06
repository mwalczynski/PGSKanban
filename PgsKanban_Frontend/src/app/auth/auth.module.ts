import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ActivationAccountService } from './login/activation-account.service';
import { ActivationAccountResolver } from './login/activation-account.resolver.service';
import { AuthService } from './auth.service';
import { AuthGuard } from './auth.guard';
import { NotAuthGuard } from './notAuth.guard';
import { LoginService } from './login/login.service';
import { RegisterService } from './register/register.service';
import { ResetPasswordService } from './reset-password/reset-password.service';
import { MaterialModule } from '../shared/material.module';
import { ModalsModule } from '../modals/modals.module';
import { LocalLoginProviderGuard } from './local-login-provider.guard';
import { OpenIdComponent } from './external-login/open-id/open-id.component';
import { LoadingModule } from 'ngx-loading';
import { ExternalLoginGuard } from './external-login/external-login.guard';
import { RECAPTCHA_GLOBAL_SETTINGS } from '../shared/constants';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { RecaptchaModule, RECAPTCHA_SETTINGS } from 'ng-recaptcha';
import { ServicesModule } from '../services/services.module';
import { LoginComponent } from './login/login.component';
import { RecoverPasswordComponent } from './recover-password/recover-password.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import {DirectivesModule} from '../directives/directives.module';
import {GoogleResolverService } from './external-login/google-resolver.service';
import {OpenIdResolverService } from './external-login/openId-resolver.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        LoadingModule,
        DirectivesModule,
        MaterialModule,
        ModalsModule,
        RecaptchaFormsModule,
        RecaptchaModule.forRoot(),
        RouterModule,
        ServicesModule
    ],
    declarations: [
        LoginComponent,
        OpenIdComponent,
        RecoverPasswordComponent,
        RegisterComponent,
        ResetPasswordComponent
    ],
    providers: [
        AuthGuard,
        AuthService,
        ExternalLoginGuard,
        LocalLoginProviderGuard,
        LoginService,
        NotAuthGuard,
        RegisterService,
        ResetPasswordService,
        ActivationAccountService,
        ActivationAccountResolver,
        LocalLoginProviderGuard,
        {
            provide: RECAPTCHA_SETTINGS,
            useValue: RECAPTCHA_GLOBAL_SETTINGS,
        },
        OpenIdResolverService,
        GoogleResolverService
    ]
})
export class AuthModule {
}
