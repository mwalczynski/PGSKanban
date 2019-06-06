import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
@Injectable()
export class SharedService {
    private foldHandler = new Subject<any>();
    onSidebarsFold = this.foldHandler.asObservable();
    foldSidebars() {
        this.foldHandler.next();
    }
}
