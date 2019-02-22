using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Audio
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Src { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Listening { get; set; }
        public int QuantityLikes { get; set; }
        public int QuantityComments { get; set; }

        public virtual ClientProfile User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
