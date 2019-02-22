using BLL.Entities.Params;
using BLL.FiltersSound.Abstraction;
using System;
using System.Linq;

namespace BLL.FiltersSound.Implementation
{
    public class WaveChangerAmplitude : IChangerAmplitude
    {
        private ISilence _silence;
        public WaveChangerAmplitude(ISilence silence)
        {
            _silence = silence;
        }

        public byte[] MakeSoundFlowing(byte[] sound)
        {
            if (sound == null)
                throw new ArgumentException("sound is null", "sound");

            ArraySegment<byte> seg = new ArraySegment<byte>(sound, 44, sound.Length - 44);
            byte[] soundBytes = seg.ToArray();


            for (int i = 2; i < soundBytes.Length - 2; i += 2) 
            {
                short sndPre = BitConverter.ToInt16(soundBytes, i - 2);
                short sndCur = BitConverter.ToInt16(soundBytes, i);
                short sndNext = BitConverter.ToInt16(soundBytes, i + 2);

                short avgAmplitude = (short)(0.25 * sndPre + 0.333 * sndCur + 0.25 * sndNext);
                byte[] sndBytes = BitConverter.GetBytes(avgAmplitude);

                soundBytes[i] = sndBytes[0];
                soundBytes[i + 1] = sndBytes[1];
            }

            return soundBytes;
        }


        public byte[] SkipSilence(byte[] sound, short minAmplitude)
        {
            if (sound == null)
                throw new ArgumentNullException("sound");
            if (minAmplitude < 0)
                throw new ArgumentException("minAmplitude can not be less 0", "minAmplitude");

            int startPos,endPos;
            _silence.DurationSilence(sound, minAmplitude, out startPos, out endPos);

            SilencyParams param = new SilencyParams() { Sound = sound, StartPos = startPos, EndPos = endPos };
            return _silence.GetSoundWithoutSilence(param);
        }
    }
}
