namespace API.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted {get; set; } = DateTime.UtcNow;
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        
        public int AuthorId { get; set; }
        public string AuthorNickName { get; set; }

        public ICollection<PostDto> Comments { get; set; } = new List<PostDto>();

        public int LikedByCount { get; set; }
        public int DislikedByCount { get; set; }
    }
}