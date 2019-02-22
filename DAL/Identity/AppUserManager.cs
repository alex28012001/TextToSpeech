using DAL.Entities;
using Microsoft.AspNet.Identity;

namespace DAL.Identity
{
    public class AppUserManager : UserManager<User>
    {
        public AppUserManager(IUserStore<User> store) : base(store) { }          
    }
}
