using DAL.UnitOfWork;
using System;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Entities.Params.SavedSoundParam;
using BLL.Infastructure;
using System.IO;
using DAL.Entities;
using BLL.Helper;
using BLL.Dto;
using BLL.FiltersSound.Abstraction;
using System.Linq;

namespace BLL.Services.Letters
{
    public class WaveLetterService : ILetterService
    {
        private IUnitOfWork _db;
        private IFilterFactory _filterFactory;
        public WaveLetterService(IUnitOfWork db, IFilterFactory filterFactory)
        {
            _db = db;
            _filterFactory = filterFactory;
        }

        public async Task<bool> IsLetterRecordedAsync(string userName, string letterName)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            if (letterName == null)
                throw new ArgumentNullException("letterName");
          
            return await _db.Letters.
                             AnyAsync(p => p.User.UserName.Equals(userName) && p.LetterName.Equals(letterName));
        }


        public bool IsVoiceRecoreded(string userName, BLL.Entities.Language language)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");

            int countLetters = _db.Letters.Count(p => p.User.UserName.Equals(userName) && (int)p.Language == (int)language);

            int needfullCountLetters = 0;
            switch(language)
            {
                case BLL.Entities.Language.rus: needfullCountLetters = 46; break;
                case BLL.Entities.Language.en: needfullCountLetters = 26; break;
            }

            return countLetters.Equals(needfullCountLetters);
        }

        public async Task<OperationDetails> SaveLetterAsync(SaveLetterParams param)
        {
            byte[] changerSound = await FiltrationSound(param.InputStream);
            if (changerSound.Length <= 46000)
                return new OperationDetails(false, "size sound stream very small, record sound again", "InputStream");

            string userDir = $@"{AppDomain.CurrentDomain.BaseDirectory}\Letters\{param.Letter.UserName}";
            if (!Directory.Exists(userDir))
                Directory.CreateDirectory(userDir);

            string dirLanguage = $@"{userDir}\{param.Letter.Language.ToString()}";
            if (!Directory.Exists(dirLanguage))
                Directory.CreateDirectory(dirLanguage);


            string letterPath = $@"{dirLanguage}\{param.Letter.LetterName}.wav";
            string resultMessage = null;
            bool existsLetter = _db.Letters.Any(p => p.User.UserName.Equals(param.Letter.UserName) && p.LetterName.Equals(param.Letter.LetterName));

            if (!existsLetter)
            {
                Letter letter = GenericMapping<LetterDto, Letter>.Map(param.Letter);
                ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(param.Letter.UserName)).FirstOrDefault();

                if (user == null)
                    return new OperationDetails(false, "user not found", "UserName");

                letter.User = user;
                letter.Src = letterPath;
                _db.Letters.Create(letter);
                await _db.SaveAsync();
                resultMessage = "letter successfully saved";
            }
  
            Stream streamSound = new WaveMemoryStream(changerSound, new NAudio.Wave.WaveFormat(param.SampleRate, param.Channels));
            await SaveToFile(streamSound, letterPath);
            resultMessage = "letter successfully changed";

            return new OperationDetails(true, resultMessage, "");
        }

        private async Task<byte[]> FiltrationSound(Stream inputStream)
        {
            IChangerAmplitude changerAmplitude = _filterFactory.CreateChangerAmplitude();
            IVolume volume = _filterFactory.CreateVolume();

            //по стандарту длина wave файла должна быть четной, поэтому если она нечетная мы прибовляем к ней 1
            long soundBytesLength = inputStream.Length; 
            if (inputStream.Length % 2 != 0) 
                soundBytesLength++;

            byte[] streamBytes = new byte[soundBytesLength];
            await inputStream.ReadAsync(streamBytes, 0, (int)inputStream.Length); 
            byte[] soundWithoutSilence = changerAmplitude.SkipSilence(streamBytes, 290);

            //по стандарту длина wave файла должна быть четной, поэтому если она нечетная мы прибовляем к ней 1
            if (soundWithoutSilence.Length % 2 != 0)
                Array.Resize(ref soundWithoutSilence, soundWithoutSilence.Length + 1);

            byte[] louderSound = volume.SetVolume(soundWithoutSilence, 160);
           

            return louderSound;
        }

        private async Task SaveToFile(Stream audioStream, string src)
        {
            using (FileStream file = new FileStream(src, FileMode.Create))
            {
                byte[] buffer = new byte[audioStream.Length];
                await audioStream.ReadAsync(buffer, 0, buffer.Length);
                await file.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}
