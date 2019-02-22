using BLL.BuildSound.Builder;
using BLL.BuildSound.BuildSoundFactory;
using BLL.Entities;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Web;

namespace BLL.NinjectModuls
{
    public class SoundModule:NinjectModule
    {
        public override void Load()
        {
            Bind<IBuildSoundFactory>().To<BuildSoundWaveRusFactory>().When(request =>
            {
                return TestLanguage(Language.rus);
            }).InRequestScope();


            Bind<ISoundBuilder>().To<WaveSoundBuilder>().InRequestScope();
        }

        private bool TestLanguage(Language neededLanguage)
        {
            string languageStr = HttpContext.Current.Request.Form["Language"];
            Language language = (Language)Enum.Parse(typeof(Language), languageStr);
            return language == neededLanguage;
        }
    }
}


