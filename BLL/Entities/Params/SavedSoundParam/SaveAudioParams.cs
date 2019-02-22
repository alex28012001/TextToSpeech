
using BLL.Dto;

namespace BLL.Entities.Params.SavedSoundParam
{
    public abstract class SaveAudioParams 
    {
        public AudioDto Audio { get; set; }
        public SaveAudioParams(AudioDto audio)
        { 
            Audio = audio;
        }
    }
}
