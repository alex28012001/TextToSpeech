using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Letter
    {   
        [Key]
        public long Id { get; set; }       
        public string LetterName { get; set; }
        public string Src { get; set; }
        public Language Language { get; set; }
        public virtual ClientProfile User { get; set; }
    }
}
