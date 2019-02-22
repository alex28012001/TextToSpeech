using BLL.Dto;
using BLL.Entities.Params;
using BLL.Services.Likes;
using BLL.Services.Sound.GetSound;
using BLL.Services.Sound.VisualizeSound;
using BLL.Services.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Voice.Helper;
using Voice.Models;
using Voice.NinjectModules;



namespace Voice.Controllers
{
    public class soundController : Controller
    {
        private ISoundServiceFactory _soundServiceFactory;
        private ILikeService _likeService;
        private IUserService _userService;


        public soundController(ISoundServiceFactory soundServiceFactory, ILikeService likeService,
                               IUserService userService)
        {
            _soundServiceFactory = soundServiceFactory;
            _likeService = likeService;
            _userService = userService;
        }


        public ActionResult author(AuthorModel model)
        {
            if (!String.IsNullOrEmpty(model.audioTitle))
            {
                IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
                AudioDto audio = getSoundService.FindAudio(model.Name, model.audioTitle);

                if(audio == null)
                    return View("UserAudioNotExists", "_Layout", model);

                return View("track", audio);
            }
            else
            {
                if (!_userService.IsExistsUsername(model.Name))
                    return View("UserNotExists", "_Layout", model.Name);
                        
                return View("author", "_Layout", model.Name);
            }
        }



        public ActionResult search(string name)
        {
            return View((object)name);
        }


        [HttpPost]
        public ActionResult FindAudio(string audioTitle)
        {
            //TODO: доделать offset и count для возвращения аудио
            IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
            IEnumerable<AudioDto> foundAudio = getSoundService.FindAudio(audioTitle, 0, 10);

            return Json(foundAudio);
        }



        [HttpGet]
        public ActionResult latest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LatestAudio()
        {
            //TODO: доделать offset и count для возвращения аудио
            IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
            IEnumerable<AudioDto> newAudio = getSoundService.GetNewAudio(0, 10);

            return Json(newAudio);
        }



        [HttpGet]
        public ActionResult popular()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PopularAudio()
        {
            //TODO: доделать offset и count для возвращения аудио
            IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
            IEnumerable<AudioDto> popularAudio = getSoundService.GetPopularAudio(0, 10);

            return Json(popularAudio);
        }


        [HttpPost]
        public ActionResult UserAudio(string userName)
        {
            //TODO: доделать offset и count для возвращения аудио
            IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
            IEnumerable<AudioDto> userAudio = getSoundService.GetUserAudio(userName, 0, 10);

            return Json(userAudio);
        }


        [HttpGet]
        public ActionResult subs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubsAudio()
        {
            //TODO: доделать offset и count для возвращения аудио
            IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
            string userName = User.Identity.Name;
            IEnumerable<AudioDto> subsAudio = getSoundService.GetSubAudio(userName, 0, 10);

            return Json(subsAudio);
        }
        


        [HttpGet]
        public ActionResult liked()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LikedAudio()
        {
            //TODO: доделать offset и count для возвращения аудио
            IGetSoundService getSoundService = _soundServiceFactory.CreateGetSoundService();
            string userName = User.Identity.Name;
            IEnumerable<AudioDto> likedAudio = getSoundService.GetLikedAudio(userName, 0, 10);

            return Json(likedAudio);
        }




        [HttpPost]
        public async Task<ActionResult> Player(PlayerModel model)
        {
            IVisualizatorService visualizatorService = _soundServiceFactory.CreateVisualizatorService();
            DataForPlayer dataForPlayer = new DataForPlayer() { Audio = model.Audio, PlayerProperties = model.PlayerProperties };

            string userName = User.Identity.Name;
            AudioMarkParams likeParams = new AudioMarkParams(userName, model.Audio.UserName, model.Audio.Title);
            bool isLiked = await _likeService.IsLikedAsync(likeParams);
            dataForPlayer.IsLiked = isLiked;

            string audioWebSitePath = model.Audio.Src.Replace("/", "\\");
            string pathToServer = AppDomain.CurrentDomain.BaseDirectory;
            string audioServerPath = $"{pathToServer}\\{audioWebSitePath}";

            string playerHtml = RenderView.RenderViewToString(this.ControllerContext, "Player", dataForPlayer);

            IEnumerable<int> dataToVisualise = visualizatorService.Visualize(audioServerPath, model.AmountProc);
            OutputPlayer visPlayer = new OutputPlayer() { Playerhtml = playerHtml, DataToVisualize = dataToVisualise };

            return Json(visPlayer);
        }


        [HttpPost]
        public ActionResult AudioRoad(string userName, string audioTitle, int amountProc)
        {
            IVisualizatorService visualizatorService = _soundServiceFactory.CreateVisualizatorService();

            string pathToServer = AppDomain.CurrentDomain.BaseDirectory;
            var audioServerPath = $"{pathToServer}Audio\\{userName}\\{audioTitle}.wav";

            IEnumerable<int> dataToVisualise = visualizatorService.Visualize(audioServerPath, amountProc);
            return Json(dataToVisualise);
        }


        public ActionResult UserNotExists(string userName)
        {
            return View(userName);
        }

        public ActionResult UserAudioNotExists(AuthorModel model)
        {
            return View(model);
        }
    }
}
