using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Entities
{
    public class User : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
