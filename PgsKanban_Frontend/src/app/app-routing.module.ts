import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';

import { ActivationAccountResolver } from './auth/login/activation-account.resolver.service';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { RecoverPasswordComponent } from './auth/recover-password/recover-password.component';
import { MainComponent } from './main/main.component';
import { AuthGuard } from './auth/auth.guard';
import { NotAuthGuard } from './auth/notAuth.guard';
import { Error404Component } from './errors/error404/error404.component';
import { Error403Component } from './errors/error403/error403.component';
import { OpenIdComponent } from './auth/external-login/open-id/open-id.component';
import { ExternalLoginGuard } from './auth/external-login/external-login.guard';
import { OpenIdResolverService } from './auth/external-login/openId-resolver.service';
import { GoogleResolverService } from './auth/external-login/google-resolver.service';
import { LocalLoginProviderGuard } from './auth/local-login-provider.guard';

const appRoutes: Routes = [
    {path: 'login', component: LoginComponent, canActivate: [NotAuthGuard, LocalLoginProviderGuard]},
    {
        path: 'login/:token',
        component: LoginComponent,
        canActivate: [LocalLoginProviderGuard],
        resolve: {confirmationEmailResult: ActivationAccountResolver}
    },
    {path: 'register', component: RegisterComponent, canActivate: [NotAuthGuard, LocalLoginProviderGuard]},
    {path: 'main', component: MainComponent, canActivate: [AuthGuard]},
    {path: 'forgot', component: ResetPasswordComponent, canActivate: [NotAuthGuard, LocalLoginProviderGuard]},
    {path: 'reset/:token', component: RecoverPasswordComponent, canActivate: [LocalLoginProviderGuard]},
    {path: 'openid', component: OpenIdComponent, canActivate: [ExternalLoginGuard], resolve: {provider: OpenIdResolverService}},
    {path: 'google', component: OpenIdComponent, canActivate: [ExternalLoginGuard], resolve: {provider: GoogleResolverService}},
    {path: '404', component: Error404Component},
    {path: '403', component: Error403Component},
    {path: '', component: MainComponent, canActivate: [AuthGuard]},
    {path: '**', redirectTo: '/404'}
];

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes, {preloadingStrategy: PreloadAllModules})
    ],
    exports: [RouterModule]
})

export class AppRoutingModule {
}
