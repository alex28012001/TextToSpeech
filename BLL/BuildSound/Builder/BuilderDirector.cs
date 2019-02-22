using BLL.Entities;
using BLL.Entities.Params.BuildSoundParam;
using BLL.Infastructure;
using NAudio.Wave;
using System.Collections.Generic;
using System.IO;

namespace BLL.BuildSound.Builder
{
    public class BuilderDirector
    {
        private ISoundBuilder _builder;
        public BuilderDirector(ISoundBuilder builder)
        {
            _builder = builder;
        }

        public ResultDetails<Stream> BuildSound(BuildSoundParams param)
        {
            string textWithAccent = _builder.InsertAccents(param.Text);
            string parsingText = _builder.ParseText(textWithAccent);
            ReaderParams readerParam = new ReaderParams(parsingText, param.UserName, param.Language);
            ResultDetails<IEnumerable<string>> letterSrcs = _builder.ReadText(readerParam);

            if (!letterSrcs.Successed)
                return new ResultDetails<Stream>(false, letterSrcs.Message, letterSrcs.Property, null);

            byte[] soundBytes = _builder.ConcanateLetters(letterSrcs.Data);
            byte[] flowingSound = _builder.MakeSoundFlowing(soundBytes);

            byte[] fasterSound = _builder.ChangeSoundSpeed(flowingSound, 4f, param.SampleRate, param.Channels);
            byte[] louderSound = _builder.ChangeSoundVolume(fasterSound, param.Volume);

            WaveFormat waveFormat = new WaveFormat(param.SampleRate, param.Channels);
            Stream soundStream = new WaveMemoryStream(louderSound, waveFormat);

            return new ResultDetails<Stream>(true, "", "", soundStream);  
        }
    }
}
