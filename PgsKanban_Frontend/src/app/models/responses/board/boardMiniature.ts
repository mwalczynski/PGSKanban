import { UserProfile } from '../profile/userProfile';
export interface BoardMiniature {
    id: number;
    obfuscatedId: string;
    name: string;
    owner: UserProfile;
    membersCount: number;
}
