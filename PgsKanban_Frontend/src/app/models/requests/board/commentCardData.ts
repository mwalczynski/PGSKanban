import { UserProfile } from '../../responses/profile/userProfile'

export class CommentCardData {
    cardId: number;
    content: string;
    isOwner?: boolean;
    timeCreated?: Date;
    user?: UserProfile;
    externalUser?: UserProfile;
}
