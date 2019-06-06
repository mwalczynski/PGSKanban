import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ChangePasswordUserData} from '../../../models/requests/profile/changePasswordData';
import {BASE_URL} from '../../../shared/constants';
import {AddPasswordUserData} from '../../../models/requests/profile/addPasswordData';

@Injectable()
export class PasswordService {
    constructor(private http: HttpClient) {
    }

    changePassword(changePasswordUser: ChangePasswordUserData) {
        return this.http.post(`${BASE_URL}/account/changePassword`, changePasswordUser);
    }

    addPassword(addPasswordData: AddPasswordUserData) {
        return this.http.post(`${BASE_URL}/account/addPassword`, addPasswordData);
    }
}
