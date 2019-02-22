using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public ClientProfile User { get; set; }

        [Required]
        public Audio Audio { get; set; }
    }
}
