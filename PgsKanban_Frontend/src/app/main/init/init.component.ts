import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { SharedService } from './../shared.service';

@Component({
    selector: 'app-init',
    templateUrl: './init.component.html',
    styleUrls: ['./init.component.scss']
})
export class InitComponent implements OnInit {
    hasBoards: boolean;

    constructor(
        private route: ActivatedRoute,
        private sharedService: SharedService
    ) { }

    ngOnInit() {
        this.hasBoards = this.route.snapshot.data['hasBoards'];
    }

    onClickOutsideSidebar() {
        this.sharedService.foldSidebars();
    }

    onLastBoardDelete() {
        this.hasBoards = false;
    }
}

