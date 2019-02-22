using System.Collections.Generic;
using BLL.Entities;
using BLL.BuildSound.ParsingText;
using BLL.Concatenation;
using BLL.FiltersSound.Abstraction;
using BLL.BuildSound.BuildSoundFactory;
using System.IO;
using NAudio.Wave;
using BLL.BuildSound.Reader;
using BLL.Infastructure;
using BLL.Entities.Params.BuildSoundParam;
using BLL.BuildSound.Accent;

namespace BLL.BuildSound.Builder
{
    public class WaveSoundBuilder : ISoundBuilder
    {
        private IParserText _parseText;
        private IReaderText _readerText;
        private IConcatenation _concatenation;
        private IFilterFactory _filterFactory;
        private IVolume _volumeChanger;
        private IChangerAmplitude _amplitudeChanger;
        private IWordAccent _wordAccent;

        public WaveSoundBuilder(IBuildSoundFactory buildSoundFactory, IFilterFactory filterFactory)
        {

            _parseText = buildSoundFactory.CreateParserText();
            _readerText = buildSoundFactory.CreateReaderText();
            _concatenation = buildSoundFactory.CreateConcatenation();
            _wordAccent = buildSoundFactory.CreateWordAccent();
            _filterFactory = filterFactory;
            _volumeChanger = filterFactory.CreateVolume();
            _amplitudeChanger = filterFactory.CreateChangerAmplitude();
        }

        public string InsertAccents(string text)
        {
            return _wordAccent.InsertAccents(text);
        }

        public string ParseText(string text)
        {
            return _parseText.PhoneticParse(text, '\'');
        }

        public ResultDetails<IEnumerable<string>> ReadText(ReaderParams param)
        {
            return _readerText.Read(param);
        }

        public byte[] ChangeSoundVolume(byte[] soundBytes, int volumeProcent)
        {
            return _volumeChanger.SetVolume(soundBytes, volumeProcent);
        }

        public byte[] ChangeSoundSpeed(byte[] soundBytes, float temp, int sampleRate, int channels)
        {
            IChangerVoice changerVoice = _filterFactory.CreateChangerVoice(sampleRate, channels);
            changerVoice.Temp = temp;
            return changerVoice.Change(soundBytes);
        }

        public byte[] MakeSoundFlowing(byte[] soundBytes)
        {
            return _amplitudeChanger.MakeSoundFlowing(soundBytes);
        }

        public byte[] ConcanateLetters(IEnumerable<string> letterSrcs)
        {
            return _concatenation.Concatenate(letterSrcs);
        }

        public Stream BuildSound(byte[] soundBytes, int sampleRate, int channels)
        { 
            WaveFormat waveFormat = new WaveFormat(sampleRate, channels);
            return new WaveMemoryStream(soundBytes, waveFormat);
        }
    }
}
