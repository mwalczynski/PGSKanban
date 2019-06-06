export class AddPasswordUserData {
    constructor(newPassword: string, confirmPassword: string){
        this.newPassword = newPassword;
        this.confirmNewPassword = confirmPassword;
    }
    newPassword: string;
    confirmNewPassword: string;
}
