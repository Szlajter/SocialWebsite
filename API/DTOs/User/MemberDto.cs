namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }
}