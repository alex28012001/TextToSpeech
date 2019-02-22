using BLL.FiltersSound.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL.Concatenation
{
    public class ConcatenationWaveFiles : IConcatenation
    {
        private IVolume _changerVolume;
        public ConcatenationWaveFiles(IVolume changerVolume)
        {
            _changerVolume = changerVolume;
        }

        public byte[] Concatenate(IEnumerable<string> soundSrcs)
        {
            if (soundSrcs == null)
                throw new ArgumentNullException("soundSrcs");

            List<string> srcs = soundSrcs.ToList();
            string[] punctuations = new string[] { " ", ",", ".", "!", "?", ";", ":" };

            //проверка на последний знак, если не пунктуация, то добовляем ".", чтобы добавить последнее слово
            bool isPun = punctuations.Contains(srcs[srcs.Count - 1].ToString());
            if (!isPun) 
                srcs.Add(".");


            ICollection<string> lettersWordSrcs = new List<string>();
            List<byte> audio = new List<byte>();
            byte[] pause = new byte[200000];


            for (int i = 0; i < srcs.Count; i++)
            {
                bool IsPunctuation = punctuations.Contains(srcs[i].ToString());
                if(IsPunctuation)
                {
                    IList<byte[]> wordWithAccent = AddVolumeForWordAccent(lettersWordSrcs, '\'');
                    byte[] word = GetBytesSound(wordWithAccent);

                    audio.AddRange(word);
                    if (i != srcs.Count - 1)
                        audio.AddRange(pause);

                    lettersWordSrcs.Clear();
                }

                else
                {
                    lettersWordSrcs.Add(srcs[i]);
                }
            }

            return audio.ToArray();
        }
        

        //TODO: доделать размер байтового массива аудио (чётность)
        private byte[] GetBytesSound(IList<byte[]> arrays) 
        {
            if (arrays.Count == 1)
            {
                return arrays[0];
            }
            else
            {
                List<byte[]> binder = new List<byte[]>();
                List<byte> adder = new List<byte>();

                for (int i = 0; i < arrays.Count(); i += 2)
                {
                    if (i != arrays.Count() - 1)
                    {
                        
                        byte[] bytesFile1 = arrays[i];
                        byte[] bytesFile2 = arrays[i + 1];

                        int part = 3;
                        int lengthRotatesBytes = 0;
                        if (bytesFile1.Length > bytesFile2.Length)
                            lengthRotatesBytes = bytesFile2.Length / part;
                        else
                            lengthRotatesBytes = bytesFile1.Length / part;

                        lengthRotatesBytes = lengthRotatesBytes % 2 == 0 ? lengthRotatesBytes : lengthRotatesBytes - 1;

                        int halfLengthRotatesBytes = lengthRotatesBytes / 2;
                        halfLengthRotatesBytes = halfLengthRotatesBytes % 2 == 0 ? halfLengthRotatesBytes : halfLengthRotatesBytes - 1;


                        int file1Offset = bytesFile1.Length - lengthRotatesBytes;
                        byte[] partFile1 = new byte[lengthRotatesBytes];
                        Buffer.BlockCopy(bytesFile1, file1Offset, partFile1, 0, partFile1.Length);

                        byte[] binder1File1 = new byte[halfLengthRotatesBytes];
                        Buffer.BlockCopy(partFile1, 0, binder1File1, 0, binder1File1.Length);

                        byte[] binder2File1 = new byte[halfLengthRotatesBytes];
                        Buffer.BlockCopy(partFile1, halfLengthRotatesBytes, binder2File1, 0, binder1File1.Length);



                        byte[] partFile2 = new byte[lengthRotatesBytes];
                        Buffer.BlockCopy(bytesFile2, 0, partFile2, 0, partFile2.Length);

                        byte[] binder1File2 = new byte[halfLengthRotatesBytes];
                        Buffer.BlockCopy(partFile2, 0, binder1File2, 0, binder1File2.Length);

                        byte[] binder2File2 = new byte[halfLengthRotatesBytes];
                        Buffer.BlockCopy(partFile2, halfLengthRotatesBytes, binder2File2, 0, binder2File2.Length);



                        byte[] newfile1bytes = new byte[bytesFile1.Length - lengthRotatesBytes];
                        Buffer.BlockCopy(bytesFile1, 0, newfile1bytes, 0, newfile1bytes.Length);

                        byte[] newfile2bytes = new byte[bytesFile2.Length - lengthRotatesBytes];
                        Buffer.BlockCopy(bytesFile2, lengthRotatesBytes, newfile2bytes, 0, newfile2bytes.Length);


                        adder.AddRange(newfile1bytes);
                        adder.AddRange(binder1File2);
                        adder.AddRange(binder1File1);
                        adder.AddRange(binder2File2);
                        adder.AddRange(binder2File1);
                        adder.AddRange(newfile2bytes);

                        binder.Add(adder.ToArray());
                        adder.Clear();
                    }
                    else
                    {
                        binder.Add(arrays[i]);
                    }
                }
                return GetBytesSound(binder);
            }
        }

        private IList<byte[]> AddVolumeForWordAccent(IEnumerable<string> wordSrcs, char symbolAccent)
        {
            IList<byte[]> byteLetters = new List<byte[]>();
            IList<byte[]> byteLettersAccent = new List<byte[]>();

            foreach (var it in wordSrcs)
            {
                if (File.Exists(it))
                {
                    byte[] byteWaveLetter = File.ReadAllBytes(it);
                    ArraySegment<byte> seg = new ArraySegment<byte>(byteWaveLetter, 44, byteWaveLetter.Length - 44);
                    byte[] byteLetter = seg.ToArray();
                    byteLetters.Add(byteLetter);
                }
            }


            int indexAccent = wordSrcs.ToList().IndexOf(symbolAccent.ToString());
            int volumePercentForLetter = 7;

            //если ударение стоит на 1 букве, то громкость слова должна постепенно уменьшаться после ударения
            if (indexAccent == 0)
            {
                int currentPercent = 100 + (byteLetters.Count * volumePercentForLetter);

                for (int i = 0; i < byteLetters.Count; i++)
                {
                    currentPercent -= volumePercentForLetter;
                    byte[] letterWithAccent = _changerVolume.SetVolume(byteLetters[i], currentPercent);
                    byteLettersAccent.Add(letterWithAccent);
                }
            }

            //иначе она должна постепенно увеличиваться до буквы с ударением
            else if (indexAccent != -1)
            {
                int currentPercent = 100;

                for (int i = 0; i <= indexAccent; i++)
                {
                    currentPercent += volumePercentForLetter;
                    byte[] letterWithAccent = _changerVolume.SetVolume(byteLetters[i], currentPercent);
                    byteLettersAccent.Add(letterWithAccent);
                }

                for (int i = indexAccent + 1; i < byteLetters.Count; i++)
                {
                    byteLettersAccent.Add(byteLetters[i]);
                }
            }
        
            //если ударения вовсе нет
            else
            {
                for (int i = 0; i < byteLetters.Count; i++)
                {
                    byteLettersAccent.Add(byteLetters[i]);
                } 
            }

            return byteLettersAccent;
        }
    }
} 

