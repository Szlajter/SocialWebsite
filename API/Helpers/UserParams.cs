namespace API.Helpers
{
    public class UserParams: PaginationParams
    {
        public string CurrentUsername { get; set; }      
        public int UserId { get; set; } 
    }
}