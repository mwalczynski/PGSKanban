import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BASE_URL} from '../shared/constants';
import {MemberData} from '../models/requests/board/memberData';

@Injectable()
export class MemberService {

    constructor(private http: HttpClient) {
    }

    deleteMember(memberData) {
        return this.http
            .request('DELETE', `${BASE_URL}/boards/member`, {body: memberData});
    }

    addMemberToBoard(memberData: MemberData) {
        return this.http.post(`${BASE_URL}/boards/member`, memberData);
    }
}
