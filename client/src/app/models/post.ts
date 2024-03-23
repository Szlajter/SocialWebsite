// only minimum for now. will add media, comments and likes later.
export interface Post {
    id: number;
    creatorId: number;
    content: string;
    datePost: Date;
    isEdited: boolean;
    idDeleted: boolean;
}