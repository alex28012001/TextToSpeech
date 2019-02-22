using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Listening
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Audio Audio { get; set; }
        public ClientProfile User { get; set; }
    }
}
