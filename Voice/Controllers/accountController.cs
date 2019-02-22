using BLL.Dto;
using BLL.Infastructure;
using BLL.Services.Sound.Editor;
using BLL.Services.Sound.GetSound;
using BLL.Services.Users;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Voice.Models;
using Voice.NinjectModules;

namespace Voice.Controllers
{
    public class accountController : Controller
    {
        private IUserService _userService;
        private ISoundServiceFactory _soundServiceFactory;

        public accountController(IUserService userService, ISoundServiceFactory soundFactory)
        {
            _userService = userService;
            _soundServiceFactory = soundFactory;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

     
        public ActionResult Registration()
        {
           return View();
        }

        [HttpGet]
        public ActionResult IsUserNameBusy(string UserName)
        {
            bool busy = _userService.IsExistsUsername(UserName);
            return Json(!busy,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationModel reg)
        {
            if (ModelState.IsValid)
            {
                await SetInitialData();
                UserDto user = new UserDto() { UserName = reg.UserName, Password = reg.Password, Role = "user" };
                OperationDetails regResult = await _userService.CreateAsync(user);

                if (regResult.Successed)
                {
                    ClaimsIdentity claim = await _userService.AuthenticateAsync(user);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, claim);
                    return View("Successed");
                }

                ModelState.AddModelError("", regResult.Message);
                return View(reg);
            }
            else
            {
                if (!ModelState.IsValidField("UserName"))
                {
                    ModelState state;
                    ModelState.TryGetValue("UserName", out state);
                    ModelState.AddModelError("UserName", state.Errors.ToString());
                }

                if (!ModelState.IsValidField("Password"))
                {
                    ModelState state;
                    ModelState.TryGetValue("Password", out state);
                    ModelState.AddModelError("Password", state.Errors.ToString());
                }
                return View(reg);
            }
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginModel login)
        {
            await SetInitialData();
            UserDto user = new UserDto() { UserName = login.UserName, Password = login.Password };
            ClaimsIdentity claim = await _userService.AuthenticateAsync(user);
            if(claim != null)
            {
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, claim);
                return RedirectToAction("latest","sound");
            }

            ModelState.AddModelError("", "логин или пароль неверны");
            return View(login);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("latest","sound");
        }

        public ActionResult Successed()
        {
            return View();
        }

        private async Task SetInitialData()
        {
            UserDto user = new UserDto() { UserName = "lesha", Password = "alex28012001", Role = "admin" };
            string[] roles = { "user", "admin" };
            await _userService.SetInitialData(user, roles);
        }


        public ActionResult profile()
        {
            return View();
        }


        [HttpPost]
        public ActionResult YourAudio(string audioTitle)
        {
            //TODO: доделать offset и count

            IGetSoundService getSound = _soundServiceFactory.CreateGetSoundService();
            string userName = User.Identity.Name;
            IEnumerable<AudioDto> userAudio = getSound.GetUserAudio(userName, 0, 10);

            return Json(userAudio);
        }

        [HttpPost]
        public void DeleteAudio(string audioTitle)
        {
            ISoundEditorService soundEditor = _soundServiceFactory.CreateSoundEditorService();
            string userName = User.Identity.Name;
            soundEditor.DeleteAudio(userName, audioTitle);
        }
    }
}