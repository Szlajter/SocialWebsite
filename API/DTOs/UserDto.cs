namespace API.DTOs
{
    //returned after login
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string Nickname { get; set; }    
    }
}