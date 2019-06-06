import { List } from './list';
export interface Board {
    id: number;
    obfuscatedId: string;
    name: string;
    lists: List[];
    isOwner: boolean;
    isEditing?: boolean;
}
