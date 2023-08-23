using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User: IdentityUser<int>
    {
        public string NickName { get; set; }
        public DateOnly Birthdate { get; set; }
        public DateTime AccountCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public List<Photo> Photos { get; set; } = new();
        public List<UserFollow> Followers { get; set; } = new();
        public List<UserFollow> Following { get; set; } = new();
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}