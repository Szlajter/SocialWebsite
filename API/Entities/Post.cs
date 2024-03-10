namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted {get; set; } = DateTime.UtcNow;
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ParentPostId { get; set; }
        public Post ParentPost { get; set; }
        public ICollection<Post> Comments { get; set; } = new List<Post>();

        public ICollection<User> LikedBy { get; set; } = new List<User>();
        public ICollection<User> DislikedBy { get; set; } = new List<User>();
    }
}