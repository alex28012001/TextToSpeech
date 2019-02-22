using BLL.Dto;
using BLL.Entities;
using BLL.Entities.Params.SavedSoundParam;
using BLL.Infastructure;
using BLL.Services.Letters;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Voice.Controllers
{
    public class recordController : Controller
    {
        private ILetterService _letterService;

        public recordController(ILetterService letterService)
        {
            _letterService = letterService;
        }

        public ActionResult letters()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> letterIsRecord(string letterName)
        {
            bool isRecord = await _letterService.IsLetterRecordedAsync(User.Identity.Name, letterName);
            return Json(isRecord);
        }


        //TODO: сделать проверку на вес записанной буквы
        [HttpPost]
        public async Task<ActionResult> saveLetter()
        {
            HttpPostedFileBase audio = Request.Files["waveFile"];
            string word = Request.Form["word"];
            int sampleRate = Convert.ToInt32(Request.Form["sampleRate"]);
            int channels = Convert.ToInt32(Request.Form["channels"]);

            LetterDto letterDto = new LetterDto()
            { LetterName = word, Language = Language.rus, UserName = User.Identity.Name };

            SaveLetterParams param = new SaveLetterParams(audio.InputStream, letterDto, sampleRate, channels);
            OperationDetails opDetails = await _letterService.SaveLetterAsync(param);
            return Json(opDetails.Successed);
        }

    }
}