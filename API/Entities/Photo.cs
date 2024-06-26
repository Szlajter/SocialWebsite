using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int  Id { get; set; }
        public string Url { get; set; }  
        public string PublicId { get; set; }  

        public Post Post { get; set; }
        public int PostId { get; set; }
    }
}