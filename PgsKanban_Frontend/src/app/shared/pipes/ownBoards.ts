import { Pipe, PipeTransform } from '@angular/core';

import { UserBoard } from './../../models/responses/board/userboard';

@Pipe({
    name: 'owner',
    pure: false
})
export class OnlyOwnerFilter implements PipeTransform {
    transform(boards: UserBoard[]): any {
        return boards.filter(b => !b.isFavorite && b.isOwner);
    }
}
