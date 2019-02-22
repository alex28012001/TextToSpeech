using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dto
{
    public class AudioDto
    {
        //TODO: нужны ли все поля?
        public string Title { get; set; }
        public string Src { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Listening { get; set; }
        public int QuantityLikes { get; set; }
        public int QuantityComments { get; set; }
    }
}
