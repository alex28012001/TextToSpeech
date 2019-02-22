using BLL.Services.Users;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Voice.NinjectModules
{
    public class AccountModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>().InRequestScope();
        }
    }
}