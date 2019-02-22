using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DAL.Context
{
    public class VoiceContext : IdentityDbContext<User>
    {
        static VoiceContext()
        {
            Database.SetInitializer<VoiceContext>(new VoiceInitializer());
        }

        public VoiceContext() : base("DefaultConnection") { }
        public VoiceContext(string connectionString) : base(connectionString) { }


        public DbSet<ClientProfile> Clientprofiles{ get; set; }
        public DbSet<Letter> Letters { get; set; }
        public DbSet<Audio> Audio { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Listening> Listening { get; set; }
        public DbSet<Sub> Subs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WordAccent> WordAccents { get; set; }
    }
}
