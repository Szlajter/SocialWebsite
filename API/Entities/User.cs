using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; } 
        public DateOnly Birthdate { get; set; }
        public DateTime AccountCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public List<Photo> Photos { get; set; } = new();

        //it is not accurate but it's fine for now
        public int GetAge()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int age = today.Year - Birthdate.Year;
            if(Birthdate > today.AddYears(-age)) age--;

            return age;
        }
    }
}