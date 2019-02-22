using BLL.Entities;

namespace Voice.Models
{
    public class BuildSoundModel
    {
        public string Text { get; set; }
        public Language Language { get; set; }
        public int PercentVolume { get; set; }
        public int SampleRate { get; set; }
    }
}