using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int AmountSubs { get; set; }
        public virtual ICollection<Letter> Letters { get; set; }
        public virtual ICollection<Audio> Audio { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Listening> Listening { get; set; }
        public virtual ICollection<Sub> Subs  { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
