namespace API.Entities
{
    public class ProfilePicture
    {
        public int  Id { get; set; }
        public string Url { get; set; }  
        public string PublicId { get; set; }  

        public User User { get; set; }
        public int UserId { get; set; }
    }
}