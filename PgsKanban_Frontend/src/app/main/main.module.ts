import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ClickOutsideModule } from 'ng-click-outside';
import { CommonModule } from '@angular/common';
import { ElasticModule } from 'angular2-elastic';
import { RouterModule } from '@angular/router';
import { DragulaModule } from 'ng2-dragula';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ExtendedBoardsSidebarComponent } from './sidebar/extendedBoardsSidebar/extendedBoardsSidebar.component';
import { ExtendedMembersSidebarComponent } from './sidebar/extendedMembersSidebar/extendedMembersSidebar.component';
import { FirstBoardCreationComponent } from './init/firstBoardCreation/first-board-creation.component';
import { InitComponent } from './init/init.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { WelcomeComponent } from './init/welcome/welcome.component';
import { CardDetailsComponent } from './card-details/card-details.component';
import { FavoritePipe } from '../shared/pipes/favouriteBoards.pipe';
import { OnlyOwnerFilter } from '../shared/pipes/ownBoards';
import { ParticipantPipe } from '../shared/pipes/participantBoards.pipe';
import { BowserModule } from 'ngx-bowser';
import { LoadingModule } from 'ngx-loading';

import { MaterialModule } from '../shared/material.module';
import { DirectivesModule } from '../directives/directives.module';

import { AddBoardComponent } from './board/add-board/add-board.component';
import { AnonymityChangeComponent } from './profile/anonymity-change/anonymity-change.component';
import { BoardComponent } from './board/board.component';
import { CardComponent } from './card/card.component';
import { BoardMiniatureComponent } from './components/board-miniature/board-miniature.component';
import { ListComponent } from './list/list.component';
import { MainComponent } from './main.component';
import { ProfileComponent } from './profile/profile.component';
import { PasswordAddComponent } from './profile/passwords/password-add/password-add.component';
import { PasswordChangeComponent } from './profile/passwords/password-change/password-change.component';
import { ProfileEditComponent } from './profile/profile-edit/profile-edit.component';

import { HasBoardsResolver } from './has-boards-resolver.service';
import { ProfileResolverService } from './profile-resolver.service';

import { UserInfoComponent } from './user-info/user-info.component';
import { FooterComponent } from './components/footer/footer.component';
import { UserboardScrollingService } from './sidebar/extendedBoardsSidebar/userboard-scrolling.service';
import { ModalsModule } from '../modals/modals.module';
import { AuthGuard } from '../auth/auth.guard';
import { CommentComponent } from './card-details/comment/comment.component';
import { SharedService } from './shared.service';
import { MemberMiniatureComponent } from './components/member-miniature/member-miniature.component';
import { MainResolverService } from './main-resolver.service';
import { MarkdownModule } from 'angular2-markdown';
import { MarkdownInfoComponent } from './components/markdown-info/markdown-info.component';
import { AddMembersComponent } from './add-members/add-members.component';
import { MemberToAddComponent } from './components/member-to-add/member-to-add.component';
import { PlusComponent } from './components/plus/plus.component';
import { MinusComponent } from './components/minus/minus.component';
import { TooltipService } from './../services/tooltip.service';
import { FirstBoardCreationService } from './init/firstBoardCreation/first-board-creation.service';
import { BoardDndService } from './board/board-dnd.service';
import { UserInfoService } from '../services/user-info.service';
import { PasswordService } from './profile/passwords/password.service';
import { CardsService } from './card/card.service';
import { BoardResolverService } from './board/board.resolver.service';
import { CardDetailsResolverService } from './board/cardDetails.resolver.service';
import { BoardWithCardResolverService } from './board/boardWithCard.resolver.service';
import { BoardService } from '../services/board.service';
import { CardDetailsService } from './card-details/card-details.service';
import { SocketsModule } from '../sockets/sockets.module';
import { TrelloImportModule } from './trello-import/trello-import.module';

@NgModule({
    imports: [
        HttpModule,
        BrowserModule,
        BrowserAnimationsModule,
        ClickOutsideModule,
        CommonModule,
        DragulaModule,
        ElasticModule,
        FlexLayoutModule,
        FlexLayoutModule,
        FormsModule,
        ReactiveFormsModule,
        DirectivesModule,
        ModalsModule,
        MaterialModule,
        BowserModule,
        LoadingModule,
        MarkdownModule,
        TrelloImportModule,
        SocketsModule,
        RouterModule.forChild([
            {
                path: '',
                component: MainComponent,
                canActivate: [AuthGuard],
                resolve: { profile: ProfileResolverService, MainResolverService },
                children: [
                    { path: '', resolve: { hasBoards: HasBoardsResolver }, component: InitComponent },
                    {
                        path: 'board/:obfuscatedId',
                        resolve: { board: BoardResolverService },
                        component: BoardComponent,
                        data: { stateName: 'board' }
                    },
                    {
                        path: 'board/:obfuscatedId/:name',
                        resolve: { board: BoardResolverService },
                        data: { stateName: 'board' },
                        component: BoardComponent
                    },
                    {
                        path: 'card/:cardObfuscatedId',
                        resolve: { board: BoardWithCardResolverService, cardDetails: CardDetailsResolverService },
                        component: BoardComponent,
                        data: { stateName: 'boardWithCardDetails' }
                    },
                    {
                        path: 'card/:cardObfuscatedId/:cardName',
                        resolve: { board: BoardWithCardResolverService, cardDetails: CardDetailsResolverService },
                        data: { stateName: 'boardWithCardDetails' },
                        component: BoardComponent
                    },
                    { path: 'board', component: AddBoardComponent },
                    { path: 'profile', component: ProfileComponent }
                ]
            }])
    ],
    declarations: [
        AddBoardComponent,
        CardComponent,
        MainComponent,
        AnonymityChangeComponent,
        BoardComponent,
        BoardMiniatureComponent,
        ExtendedBoardsSidebarComponent,
        ExtendedMembersSidebarComponent,
        FavoritePipe,
        FirstBoardCreationComponent,
        InitComponent,
        ListComponent,
        MainComponent,
        OnlyOwnerFilter,
        ParticipantPipe,
        PasswordAddComponent,
        PasswordChangeComponent,
        ProfileComponent,
        ProfileEditComponent,
        SidebarComponent,
        CardComponent,
        UserInfoComponent,
        WelcomeComponent,
        MemberMiniatureComponent,
        FooterComponent,
        CardDetailsComponent,
        CommentComponent,
        MarkdownInfoComponent,
        AddMembersComponent,
        MemberToAddComponent,
        PlusComponent,
        MinusComponent
    ],
    providers: [
        AnonymityChangeComponent,
        BoardResolverService,
        CardDetailsResolverService,
        BoardWithCardResolverService,
        BoardService,
        CardsService,
        UserInfoService,
        ExtendedBoardsSidebarComponent,
        ExtendedMembersSidebarComponent,
        FirstBoardCreationService,
        HasBoardsResolver,
        PasswordService,
        ProfileResolverService,
        SharedService,
        SidebarComponent,
        UserInfoComponent,
        UserInfoService,
        WelcomeComponent,
        CardDetailsService,
        UserboardScrollingService,
        BoardDndService,
        MainResolverService,
        BowserModule,
        TooltipService
    ],
    entryComponents: [
        AddMembersComponent
    ]
})

export class MainModule {
}
