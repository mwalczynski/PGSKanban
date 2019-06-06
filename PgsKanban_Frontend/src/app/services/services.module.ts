import { NormalizeNameService } from './normalize-name.service';
import { ListsService } from './lists.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PreviousRouteService } from './previous-route.service';
import { BoardService } from './board.service';
import { ExternalLoginService } from './external-login.service';
import { UserInfoService } from './user-info.service';
import { MemberService } from './member.service';
import { FacebookService } from 'ngx-facebook';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [],
    providers: [
        NormalizeNameService,
        PreviousRouteService,
        BoardService,
        UserInfoService,
        ExternalLoginService,
        ListsService,
        FacebookService,
        MemberService
    ]
})
export class ServicesModule {
}
