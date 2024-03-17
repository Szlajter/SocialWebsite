namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted {get; set; } = DateTime.UtcNow;
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? ParentPostId { get; set; }
        public Post ParentPost { get; set; }

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<Post> Comments { get; set; } = new List<Post>();
        public ICollection<User> LikedBy { get; set; } = new List<User>();
        public ICollection<User> DislikedBy { get; set; } = new List<User>();
    }
}