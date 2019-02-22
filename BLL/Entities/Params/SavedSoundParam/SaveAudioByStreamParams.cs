using BLL.Dto;
using System.IO;

namespace BLL.Entities.Params.SavedSoundParam
{
    public class SaveAudioByStreamParams : SaveAudioParams
    {
        public Stream InputStream { get; set; }

        public SaveAudioByStreamParams(Stream inputStream, AudioDto audio)
            : base(audio)
        {
            InputStream = inputStream;
        }
    }
}
