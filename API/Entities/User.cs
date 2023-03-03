using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}