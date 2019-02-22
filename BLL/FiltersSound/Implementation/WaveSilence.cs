using BLL.Entities.Params;
using BLL.FiltersSound.Abstraction;
using System;
using System.Linq;

namespace BLL.FiltersSound.Implementation
{
    public class WaveSilence : ISilence
    {
        public void DurationSilence(byte[] sound, short minAmplitude, out int startPos, out int endPos)
        {
            ArraySegment<byte> seg = new ArraySegment<byte>(sound, 44, sound.Length - 44);
            byte[] soundBytes = seg.ToArray();

            bool audioHaveSound = false;
            startPos = 0;
            endPos = soundBytes.Length - 1;

            for (int i = 0; i < soundBytes.Length; i += 2)
            {
                short snd = BitConverter.ToInt16(soundBytes, i);
                if (snd > minAmplitude)
                {
                    startPos = i;
                    audioHaveSound = true; 
                    break;
                }
            }

            for (int i = soundBytes.Length - 1; i >= 0; i -= 2)
            {
                short snd = BitConverter.ToInt16(soundBytes, i - 1);
                if (snd > minAmplitude)
                {
                    endPos = i;
                    break;
                }
            }

            if (audioHaveSound == false) //если в аудио только тишина, то начало звука, это конец аудио т.е его нет
                startPos = soundBytes.Length - 1;
        }


        public byte[] GetSoundWithoutSilence(SilencyParams param)
        {
            byte[] newarray = new byte[param.EndPos - param.StartPos]; 
            for (int n = 0 , o = param.StartPos; o < param.EndPos; o++, n++)
            {
                newarray[n] = param.Sound[o];
            }

            return newarray;
        }    
    }
}
