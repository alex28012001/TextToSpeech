using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DAL.Context;
using System.Data.Entity;
using DAL.Dto;

namespace DAL.Repository
{
    public class LikeRepository : IRepository<Like>
    {
        private VoiceContext _db;
        public LikeRepository(DbContext db)
        {
            _db = (VoiceContext)db;
        }

        public void Create(Like item)
        {
            _db.Likes.Add(item);
        }

        public IEnumerable<Like> Find(Func<Like, bool> predicate)
        {
            return _db.Likes.Include("Audio").Where(predicate);
                                  
        }

        public IQueryable<Like> FindWithExpressionsTree(Expression<Func<Like, bool>> predicate)
        {
            return _db.Likes.Include("Audio").Where(predicate);             
        }
        
    
        public IEnumerable<Like> GetAll()
        {
            return _db.Likes.Include("Audio");                                           
        }

        public IQueryable<Like> GetAllWithExpressionsTree()
        {
            return _db.Likes.Include("Audio");
        } 

        public void Remove(Like item)
        {
            _db.Likes.Remove(item);
        }

        public bool Any(Expression<Func<Like, bool>> predicate)
        {
            return _db.Likes.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<Like, bool>> predicate)
        {
            return await _db.Likes.AnyAsync(predicate);
        }

        public void Update(Like item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public int Count(Expression<Func<Like, Boolean>> predicate)
        {
            return _db.Likes.Count(predicate);
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
