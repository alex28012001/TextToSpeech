using BLL.Dto;
using System.IO;
using System;

namespace BLL.Entities.Params.SavedSoundParam
{
    public class SaveLetterParams 
    {
        private Stream _inputStream;
        private int _sampleRate;
        private int _channels;

        public LetterDto Letter { get; set; }
        public Stream InputStream {
            get { return _inputStream; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("inputStream");
                _inputStream = value;
            }
        }
        public int SampleRate
        {
            get { return _sampleRate; }

            set
            {
                if (value < 0)
                    throw new ArgumentException("sampleRate can not be less 0");
                _sampleRate = value;
            }
        }

        public int Channels
        {
            get { return _channels; }

            set
            {
                if (value < 1)
                    throw new ArgumentException("channels can not be less 1");
                _channels = value;
            }
        }

        public SaveLetterParams(Stream inputStream, LetterDto letter, int sampleRate, int channels)
        {
            Letter = letter;
            InputStream = inputStream;
            SampleRate = sampleRate;
            Channels = channels;
        }
    }
}
