using BLL.FiltersSound.Abstraction;
using System;

namespace BLL.FiltersSound.Implementation
{
    public class WaveVolume : IVolume
    {
        public byte[] SetVolume(byte[] soundBytes, int percent)
        {
            if (soundBytes == null)
                throw new ArgumentNullException("soundBytes");
            if (percent < 0)
                throw new ArgumentException("procent can not be less 0", "procent");


            for (int i = 0; i < soundBytes.Length; i += 2)
            {
                short snd = BitConverter.ToInt16(soundBytes, i);
                int proc = snd * percent / 100;

                byte[] byteSnd = BitConverter.GetBytes(proc);
                soundBytes[i] = byteSnd[0];
                soundBytes[i + 1] = byteSnd[1];
            }
     
            return soundBytes;
        }
    }
}
        
    

