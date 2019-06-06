import { UserProfile } from '../profile/userProfile';
export interface Comment {
    cardId: number;
    content: string;
    isOwner?: boolean;
    timeCreated?: Date;
    user?: UserProfile;
    externalUser?: UserProfile;
}
