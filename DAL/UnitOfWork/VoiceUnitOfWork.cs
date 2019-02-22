using System;
using System.Threading.Tasks;
using DAL.Repository;
using DAL.Context;
using System.Data.Entity;
using DAL.Entities;
using Microsoft.AspNet.Identity;
using DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.UnitOfWork
{
    public class VoiceUnitOfWork : IUnitOfWork
    {
        private DbContext _db;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private IRepository<ClientProfile> _clientProfileRepository;
        private IRepository<Letter> _letterRepository;
        private IRepository<Audio> _audioRepository;
        private IRepository<Like> _likeRepository;
        private IRepository<Listening> _listeningRepository;
        private IRepository<Sub> _subRepository;
        private IRepository<Comment> _commentRepository;
        private IRepository<WordAccent> _wordAccentRepository;

        public VoiceUnitOfWork()
        {
            _db = new VoiceContext();
            _userManager = new AppUserManager(new UserStore<User>(_db));
            _roleManager = new AppRoleManager(new RoleStore<Role>(_db));
        }

        public VoiceUnitOfWork(string connectionString)
        {
            _db = new VoiceContext(connectionString);
        }

        public IRepository<ClientProfile> ClientProfiles
        {
            get
            {
                if (_clientProfileRepository == null)
                    _clientProfileRepository = new ClientProfileRepository(_db);
                return _clientProfileRepository;
            }
        }

        public IRepository<Letter> Letters
        {
            get
            {
                if (_letterRepository == null)
                    _letterRepository = new LetterRepository(_db);
                return _letterRepository;
            }
        }

        public IRepository<Audio> Audio
        {
            get
            {
                if (_audioRepository == null)
                    _audioRepository = new AudioRepository(_db);
                return _audioRepository;
            }
        }

        public IRepository<Like> Likes
        {
            get
            {
                if (_likeRepository == null)
                    _likeRepository = new LikeRepository(_db);
                return _likeRepository;
            }
        }


        public IRepository<Listening> Listening
        {
            get
            {
                if (_listeningRepository == null)
                    _listeningRepository = new ListeningRepository(_db);
                return _listeningRepository; 
            }
            
        }

        public IRepository<Sub> Subs
        {
            get
            {
                if (_subRepository == null)
                    _subRepository = new SubRepository(_db);
                return _subRepository;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_db);
                return _commentRepository;
            }
        }

        public IRepository<WordAccent> WordAccents
        {
            get
            {
                if (_wordAccentRepository == null)
                    _wordAccentRepository = new WordAccentRepository(_db);
                return _wordAccentRepository;
            }
        }


        public UserManager<User> UserManager
        {
            get {return _userManager; }
        }

        public RoleManager<Role> RoleManager
        {
            get { return _roleManager; }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private bool disposed = false;
        private void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    _db.Dispose();
                    _roleManager.Dispose();
                    _userManager.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
