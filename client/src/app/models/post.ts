// only minimum for now. will add media, comments and likes later.
export interface Post {
    id: number;
    authorId: number;
    authorUsername: string;
    authorNickname: string;
    authorPhotoUrl: string;
    content: string;
    datePosted: Date;
    isEdited: boolean;
    idDeleted: boolean;
    commentCount: number;
    comments: Post[];
    likedByCount: number;
    dislikedByCount: number;
    hasLiked: boolean;
    hasDisliked: boolean;
}