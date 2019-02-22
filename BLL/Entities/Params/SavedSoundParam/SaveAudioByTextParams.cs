using BLL.Dto;

namespace BLL.Entities.Params.SavedSoundParam
{
    public class SaveAudioByTextParams : SaveAudioParams
    {
        public string Text { get; set; }
        public Language Language { get; set; }
        public int SampleRate { get; set; }

        public SaveAudioByTextParams(string text, AudioDto audio, Language lang, int sampleRate)
            : base(audio)
        {
            Text = text;
            Language = lang;
            SampleRate = sampleRate;
        }
    }
}
