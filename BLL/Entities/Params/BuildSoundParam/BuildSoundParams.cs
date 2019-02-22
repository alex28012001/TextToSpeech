namespace BLL.Entities.Params.BuildSoundParam
{
    public class BuildSoundParams
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public int SampleRate { get; set; }
        public int Channels { get; set; }
        public int Volume { get; set; }
        public Language Language { get; set; }

        public BuildSoundParams()
        {
            SampleRate = 44100;
            Channels = 2;
            Volume = 100;
        }
    }
}
