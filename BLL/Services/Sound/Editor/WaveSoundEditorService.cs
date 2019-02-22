using System.Linq;
using System.Threading.Tasks;
using System.IO;
using DAL.UnitOfWork;
using DAL.Entities;
using BLL.Dto;
using BLL.Helper;
using BLL.Entities.Params.SavedSoundParam;
using BLL.Entities.Params.BuildSoundParam;
using System;
using BLL.Infastructure;
using BLL.BuildSound.Builder;

namespace BLL.Services.Sound.Editor
{
    public class WaveSoundEditorService : ISoundEditorService
    {
        private IUnitOfWork _db;
        private ISoundBuilder _soundBuilder;

        public WaveSoundEditorService(IUnitOfWork db, ISoundBuilder soundBuilder)                                                                
        {
            _db = db;
            _soundBuilder = soundBuilder;
        }

        public async Task<OperationDetails> SaveAudioByStreamAsync(SaveAudioByStreamParams param)
        {
            return await SaveAsync(param, param.InputStream);    
        }

        public async Task<OperationDetails> SaveAudioByTextAsync(SaveAudioByTextParams param)
        {
            BuilderDirector builderDirector = new BuilderDirector(_soundBuilder);
            
            BuildSoundParams buildSoundParam = new BuildSoundParams()
            { Text = param.Text, Language = param.Language, SampleRate = param.SampleRate, UserName = param.Audio.UserName };
            ResultDetails<Stream> resultBuildingSound = builderDirector.BuildSound(buildSoundParam);

            if(!resultBuildingSound.Successed)
                return new OperationDetails(false, resultBuildingSound.Message, resultBuildingSound.Property);

            return await SaveAsync(param, resultBuildingSound.Data);
        }

        private async Task<OperationDetails> SaveAsync(SaveAudioParams param, Stream audioStream)
        {
            if (audioStream == null)
                throw new ArgumentNullException("audioStream");
            if (String.IsNullOrEmpty(param.Audio.Title))
                return new OperationDetails(false, "Audio title can not be empty", "Title");


            string userDir = $@"{AppDomain.CurrentDomain.BaseDirectory}\Audio\{param.Audio.UserName}";
            if (!Directory.Exists(userDir))
                Directory.CreateDirectory(userDir);

            string audioName = $"{param.Audio.Title}.wav";
            string audioPathToServer = $@"/Audio/{param.Audio.UserName}/{audioName}";
            string audioPath = $@"{userDir}\{audioName}";

            Audio audio = GenericMapping<AudioDto, Audio>.Map(param.Audio);
            ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(param.Audio.UserName)).FirstOrDefault();

            if (user == null)
                return new OperationDetails(false, "user not found", "UserName");

            audio.User = user;
            audio.Src = audioPathToServer;
            _db.Audio.Create(audio);
            await _db.SaveAsync();

            await SaveToFile(audioStream, audioPath);
            audioStream.Position = 0;

            return new OperationDetails(true, "audio successfully saved", "");
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

        public void DeleteAudio(string userName, string audioTitle)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            if (audioTitle == null)
                throw new ArgumentNullException("audioTitle");

            Audio audio = _db.Audio.FindWithExpressionsTree
                                   (p => p.User.UserName.Equals(userName) && p.Title.Equals(audioTitle)).FirstOrDefault();

            if (audio == null)
                throw new ArgumentException("audio not found","userName or audioTitle");
            
            _db.Audio.Remove(audio);
            _db.Save();
        }
    }
}
