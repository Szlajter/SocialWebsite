namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        // TODO: media attachment like photos
        // TODO: likes and comments
        public User Author { get; set; }
        public DateTime DatePosted {get; set; } = DateTime.UtcNow;
    }
}