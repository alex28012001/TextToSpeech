using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL.Services.Sound.VisualizeSound
{
    public class WaveVisualizatorService : IVisualizatorService
    {
        public IEnumerable<int> Visualize(string audioPath, int amountProc)
        {
            if (!File.Exists(audioPath))
                throw new FileNotFoundException("file not found", audioPath);

            if (amountProc < 0)
                throw new ArgumentException("amount procents can not be less 0", "amountProc");


            byte[] file = File.ReadAllBytes(audioPath);
            ArraySegment<byte> seg = new ArraySegment<byte>(file, 44, file.Length - 44);
            byte[] fileBytes = seg.ToArray();

            int proc = fileBytes.Length / amountProc;
            int volumeProc = 0;
            int currentProc = proc;
            ICollection<int> visualizator = new List<int>();

            int count = 0;
            for (int i = 0; i < fileBytes.Length; i += 2)
            {
                short snd = BitConverter.ToInt16(fileBytes, i);
                if (snd > 0)
                {
                    float x = Convert.ToSingle(snd);
                    int db = (int)(20 * Math.Log10(x));
                    volumeProc += db;
                    ++count;
                }

                if (i >= currentProc && count != 0)
                {
                    currentProc += proc;
                    int avarageVolumeOneProc = volumeProc / count;
                    visualizator.Add(avarageVolumeOneProc);
                    volumeProc = 0;
                    count = 0;
                }
            }
            return visualizator;
        }
    }
}
