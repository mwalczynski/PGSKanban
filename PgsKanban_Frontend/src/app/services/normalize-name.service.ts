import { Injectable } from '@angular/core';

@Injectable()
export class NormalizeNameService {

    normalizeName(name) {
        let normalizedName = name.replace(/[^\w\s]/gi, '').replace(/\s+/g, '-').toLowerCase();
        if (normalizedName === '') {
            normalizedName = 'untitled';
        }
        return normalizedName;
    }
}
