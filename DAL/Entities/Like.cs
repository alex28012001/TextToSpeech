using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Audio Audio { get; set; }
        public virtual ClientProfile User { get; set; }
    }
}
