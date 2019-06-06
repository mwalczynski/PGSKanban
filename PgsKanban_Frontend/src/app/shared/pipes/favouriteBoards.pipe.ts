import {Pipe, PipeTransform} from '@angular/core';

import {UserBoard} from './../../models/responses/board/userboard';

@Pipe({
    name: 'favorite',
    pure: false
})
export class FavoritePipe implements PipeTransform {
    transform(boards: UserBoard[]): any {
        return boards
            .filter(b => b.isFavorite)
            .sort((a, b) => {
                return b.lastTimeSetFavorite > a.lastTimeSetFavorite ? 1 : -1;
            });
    }
}
