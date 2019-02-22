using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Sub
    {
        [Key]
        public int Id { get; set; }
        public virtual ClientProfile User { get; set; }
        public virtual ClientProfile Singer { get; set; }
    }
}
