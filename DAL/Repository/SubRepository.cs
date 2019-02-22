using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DAL.Context;
using System.Data.Entity;

namespace DAL.Repository
{
    public class SubRepository : IRepository<Sub>, ISubRepository
    {
        private VoiceContext _db;
        public SubRepository(DbContext db)
        {
            _db = (VoiceContext)db;
        }


        public void Create(Sub item)
        {
            _db.Subs.Add(item);
        }

        public void Remove(Sub item)
        {
            _db.Subs.Remove(item);
        }

        public int Count(Expression<Func<Sub, bool>> predicate)
        {
            return _db.Subs.Count(predicate);
        }

        public IEnumerable<Sub> Find(Func<Sub, bool> predicate)
        {
            return _db.Subs.Include("Singer").Where(predicate);
        }

        public IQueryable<Sub> FindWithExpressionsTree(Expression<Func<Sub, bool>> predicate)
        {
            return _db.Subs.Include("Singer").Where(predicate);             
        }

        public bool Any(Expression<Func<Sub, bool>> predicate)
        {
            return _db.Subs.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<Sub, bool>> predicate)
        {
            return await _db.Subs.AnyAsync(predicate);
        }

        public IEnumerable<Sub> GetAll()
        {
            return _db.Subs.Include("User").Include("Singer");
        }

        public IQueryable<Sub> GetAllWithExpressionsTree()
        {
            return _db.Subs.Include("User").Include("Singer");
        }

        public void Update(Sub item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public IQueryable<Audio> FindWithExpressionTreeSubAudio(Expression<Func<Sub, bool>> predicate)
        {
            return _db.Subs.Where(predicate)
                           .SelectMany(p => p.Singer.Audio)
                           .Include("User");
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
