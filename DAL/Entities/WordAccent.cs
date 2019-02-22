using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class WordAccent
    {
        [Key]
        public int Id { get; set; }
        public string Word { get; set; }
        public string Accent { get; set; }
    }
}
