using DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Identity
{
    public class AppRoleManager : RoleManager<Role>
    {
        public AppRoleManager(RoleStore<Role> store) : base(store) { } 
    }
}
