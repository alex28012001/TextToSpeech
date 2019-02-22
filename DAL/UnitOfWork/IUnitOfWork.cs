using DAL.Entities;
using DAL.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ClientProfile> ClientProfiles { get; }
        IRepository<Letter> Letters { get; }
        IRepository<Audio> Audio { get; }
        IRepository<Like> Likes { get; }
        IRepository<Listening> Listening { get; }
        IRepository<Sub> Subs { get; }
        IRepository<Comment> Comments { get; }
        IRepository<WordAccent> WordAccents { get; }
        UserManager<User> UserManager { get; }
        RoleManager<Role> RoleManager { get; }

        void Save();
        Task SaveAsync();
    }
}
