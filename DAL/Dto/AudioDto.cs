using System;

namespace DAL.Dto
{
    public class AudioDto
    {
        public string Title { get; set; }
        public string Src { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Listening { get; set; }
        public int QuantityLikes { get; set; }
    }
}
