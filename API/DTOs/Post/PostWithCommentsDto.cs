namespace API.DTOs
{
    public class PostWithCommentsDto
    {
         public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted {get; set; }
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        
        public int AuthorId { get; set; }
        public string AuthorNickname { get; set; }
        public string AuthorUsername { get; set; }
        public string AuthorPhotoUrl { get; set; }

        public int? ParentPostId { get; set; }
        public int commentCount { get; set; }
        public ICollection<PostDto> Comments { get; set; }

        public int LikedByCount { get; set; }
        public int DislikedByCount { get; set; }
        public bool hasLiked { get; set; }
        public bool hasDisliked { get; set; }
    }
}