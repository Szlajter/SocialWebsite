namespace API.DTOs
{
    //returned after login
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}