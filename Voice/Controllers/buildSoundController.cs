using BLL.Dto;
using BLL.Entities;
using BLL.Entities.Params.BuildSoundParam;
using BLL.Entities.Params.SavedSoundParam;
using BLL.Infastructure;
using BLL.Services.Letters;
using BLL.Services.Sound.BuildSound;
using BLL.Services.Sound.Editor;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Voice.Models;
using Voice.NinjectModules;

namespace Voice.Controllers
{
    public class buildSoundController : Controller
    {
        private ISoundServiceFactory _soundServiceFactory;
        private ILetterService _letterService;

        public buildSoundController(ISoundServiceFactory soundServiceFactory, ILetterService letterService)
        {
            _soundServiceFactory = soundServiceFactory;
            _letterService = letterService;
        }

        public ActionResult create()
        {
            return View();
        }

        //TASK: сделать выбор языка
        //TASK: сделать проверку на запись всех букв по заданному языку
        [HttpPost]
        public ActionResult create(BuildSoundModel model)
        {
            IBuilderSoundService builderSoundService = _soundServiceFactory.CreateBuilderSoundService();

            BuildSoundParams buildSoundParam = new BuildSoundParams()
            { Text = model.Text, Language = model.Language, UserName = User.Identity.Name,
              Volume = model.PercentVolume, SampleRate = model.SampleRate };

            ResultDetails<Stream> resultBuildingSound = builderSoundService.BuildSound(buildSoundParam);

            if (!resultBuildingSound.Successed)
                return new EmptyResult();  

            Stream soundStream = resultBuildingSound.Data;
            Response.AppendHeader("Content-Length", soundStream.Length.ToString());
            Response.AppendHeader("Accept-Ranges", "bytes");

            return File(soundStream, "audio/wav");
        }


        [HttpPost]
        public ActionResult IsVoiceRecorded(Language language)
        {
            string userName = User.Identity.Name;
            bool isVoiceRecorded = _letterService.IsVoiceRecoreded(userName, language);
            return Json(isVoiceRecorded);
        }

      
        [HttpPost]
        public async Task<ActionResult> saveAudioByStream()
        {
            ISoundEditorService soundEditor = _soundServiceFactory.CreateSoundEditorService();

            HttpPostedFileBase audio = Request.Files["waveFile"];
            string title = Request.Form["title"];
            string text = Request.Form["text"];

            AudioDto audioDto = new AudioDto()
            { Title = title,Text = text, UserName = User.Identity.Name,Date = DateTime.UtcNow };
            SaveAudioByStreamParams param = new SaveAudioByStreamParams(audio.InputStream, audioDto);
            OperationDetails resultSaveAudio = await soundEditor.SaveAudioByStreamAsync(param);

            string resultStr = resultSaveAudio.Successed ? "Аудио успешно сохранено" : "текст должен содержать хотя-бы 1 алфавитно-символьный символ";
            return Content(resultStr);
        }


        [HttpPost]
        public async Task<ActionResult> saveAudioByText(string text, string title,Language Language, int sampleRate)
        {
            ISoundEditorService soundEditor = _soundServiceFactory.CreateSoundEditorService();

            AudioDto audioDto = new AudioDto()
            { Title = title, Text = text, UserName = User.Identity.Name, Date = DateTime.UtcNow };
            SaveAudioByTextParams param = new SaveAudioByTextParams
            (text, audioDto, Language.rus, sampleRate);

            OperationDetails resultSaveAudio = await soundEditor.SaveAudioByTextAsync(param);

            string resultStr = resultSaveAudio.Successed ? "Аудио успешно сохранено" : "текст должен содержать хотя-бы 1 алфавитно-символьный символ";
            return Content(resultStr);
        }
    }
}