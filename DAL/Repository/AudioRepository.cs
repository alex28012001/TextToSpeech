using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Context;
using System.Data.Entity;
using DAL.Dto;

namespace DAL.Repository
{
    public class AudioRepository : IRepository<Audio>
    {
        private VoiceContext _db;
        public AudioRepository(DbContext db)
        {
            _db = db as VoiceContext;
        }
        public void Create(Audio item)
        {
            _db.Audio.Add(item);
        }

        public void Remove(Audio item)
        {
            _db.Audio.Remove(item);
        }

        public IEnumerable<Audio> Find(Func<Audio, bool> predicate)
        {
            return _db.Audio.Where(predicate);
        }

        public IQueryable<Audio> FindWithExpressionsTree(Expression<Func<Audio, bool>> predicate)
        {
            return _db.Audio.Include("User").Where(predicate);               
        }

        public void Update(Audio item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Audio> GetAll()
        {
            return _db.Audio.Include("User");
        }


        public IQueryable<Audio> GetAllWithExpressionsTree()
        {
            return _db.Audio.Include("User");
        }


        public async Task<bool> AnyAsync(Expression<Func<Audio, Boolean>> predicate)
        {
            return await _db.Audio.AnyAsync(predicate);
        }

        public bool Any(Expression<Func<Audio, Boolean>> predicate)
        {
            return _db.Audio.Any(predicate);
        }

        public int Count(Expression<Func<Audio, Boolean>> predicate)
        {
            return _db.Audio.Count(predicate);
        }

        public void Dispose()
        {
            _db.Dispose(); 
        }
    }
}
