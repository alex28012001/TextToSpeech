using System.Collections.Generic;

namespace Voice.Models
{
    public class OutputPlayer
    {
        public string Playerhtml { get; set; }
        public IEnumerable<int> DataToVisualize { get; set; }
    }
}