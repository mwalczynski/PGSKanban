import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../shared/constants';
import { ImportStatistics } from '../../models/responses/board/import-statistics';

@Injectable()
export class ImportService {
    constructor(private http: HttpClient) {
    }

    importBoard(file: any, boardName: string) {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('boardName', boardName);
        return this.http.post(`${BASE_URL}/import`, formData).map((response) => response as ImportStatistics);
    }
}
