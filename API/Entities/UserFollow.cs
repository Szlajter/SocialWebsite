namespace API.Entities
{
    public class UserFollow
    {
        public int SourceUserId { get; set; }
        public int TargetUserId { get; set; }
        public User SourceUser { get; set; }
        public User TargetUser { get; set; }
    }
}