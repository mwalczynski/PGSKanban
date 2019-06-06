export interface UserProfile {
    firstName: string;
    lastName: string;
    isProfileAnonymous: boolean;
    hashMail: string;
    email: string;
    pictureSrc?: string;
    hasPassword?: boolean;
}
