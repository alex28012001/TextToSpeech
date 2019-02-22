using BLL.Entities;
using BLL.Entities.Params.BuildSoundParam;
using BLL.Infastructure;
using System.Collections.Generic;
using System.IO;

namespace BLL.BuildSound.Builder
{
    public interface ISoundBuilder
    {
        string InsertAccents(string text);
        string ParseText(string text);
        ResultDetails<IEnumerable<string>> ReadText(ReaderParams param);
        byte[] ChangeSoundVolume(byte[] soundBytes, int volumeProcent);
        byte[] ChangeSoundSpeed(byte[] soundBytes, float temp, int sampleRate, int channels);
        byte[] MakeSoundFlowing(byte[] soundBytes);
        byte[] ConcanateLetters(IEnumerable<string> letterSrcs);
        Stream BuildSound(byte[] soundBytes, int sampleRate, int channels);
    }
}
