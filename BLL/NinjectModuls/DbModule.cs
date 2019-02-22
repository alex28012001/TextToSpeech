using DAL.UnitOfWork;
using Ninject.Modules;
using Ninject.Web.Common;

namespace BLL.NinjectModuls
{
    public class DbModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<VoiceUnitOfWork>().InRequestScope();
        }
    }
}
