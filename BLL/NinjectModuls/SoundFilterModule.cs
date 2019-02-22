using Ninject.Modules;
using BLL.FiltersSound.Abstraction;
using BLL.FiltersSound.Implementation;
using Ninject.Web.Common;
using Ninject.Extensions.Factory;

namespace BLL.NinjectModuls
{
    public class SoundFilterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IVolume>().To<WaveVolume>().InRequestScope();
            Bind<ISilence>().To<WaveSilence>().InRequestScope();
            Bind<IChangerAmplitude>().To<WaveChangerAmplitude>().InRequestScope();
            Bind<IChangerVoice>().To<WaveChangerVoice>().InRequestScope();
            Bind<IFilterFactory>().ToFactory().InRequestScope();
        }
    }
}
