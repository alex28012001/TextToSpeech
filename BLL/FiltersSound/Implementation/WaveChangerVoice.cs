using BLL.FiltersSound.Abstraction;
using SoundTouchNet;
using System;
using System.Linq;

namespace BLL.FiltersSound.Implementation
{
    public class WaveChangerVoice : IChangerVoice
    {
        private SoundStretcher _soundStretcher;

        private float _temp;
        private float _pitch;

        public float Temp
        {
            get { return _temp; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("temp can not be less 0", "Temp");
                _temp = value;
            }
        }

        public float Pitch
        {
            get { return _pitch; }      
            set
            {
                if (value < 0)
                    throw new ArgumentException("temp can not be less 0", "Pitch");
                _pitch = value;
            }
        }

      

        public WaveChangerVoice(int sampleRate, int channels)
        {
            if (sampleRate < 0)
                throw new ArgumentException("sampleRate can not be less 0");

            if (channels < 1)
                throw new ArgumentException("channels can not be less 1");

            _soundStretcher = new SoundStretcher(sampleRate, channels);
            Temp = 1;
            Pitch = 1;
        }

        public byte [] Change(byte [] soundBytes)
        {
            if (soundBytes == null)
                throw new ArgumentNullException("sountBytes");

            _soundStretcher.Tempo = Temp;
            _soundStretcher.Pitch = Pitch;

            _soundStretcher.PutSamplesFromBuffer(soundBytes, 0, soundBytes.Length);
            byte[] changerSound = new byte[soundBytes.Length];
            int avaibleBytes = _soundStretcher.ReceiveSamplesToBuffer(changerSound, 0, changerSound.Length);

            if (avaibleBytes > 0)
            {
                ArraySegment<byte> seg = new ArraySegment<byte>(changerSound, 0, avaibleBytes);
                return seg.ToArray();
            }
            return soundBytes;           
        }
    }
}
