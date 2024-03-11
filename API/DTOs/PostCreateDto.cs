namespace API.DTOs
{
    public class PostCreateDto
    {
        public string Content { get; set; }
        public int? ParentPostId { get; set; }
    }
}