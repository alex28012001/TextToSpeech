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
    public class ListeningRepository : IRepository<Listening>
    {
        private VoiceContext _db;
        public ListeningRepository(DbContext db)
        {
            _db = (VoiceContext)db;
        }
        public void Create(Listening item)
        {
            _db.Listening.Add(item);
        }

        public void Remove(Listening item)
        {
            _db.Listening.Remove(item);
        }

        public IEnumerable<Listening> Find(Func<Listening, bool> predicate)
        {
            return _db.Listening.Where(predicate);
        }

        public IQueryable<Listening> FindWithExpressionsTree(Expression<Func<Listening, bool>> predicate)
        {
            return _db.Listening.Where(predicate);
        }


        public bool Any(Expression<Func<Listening, bool>> predicate)
        {
            return _db.Listening.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<Listening, bool>> predicate)
        {
            return await _db.Listening.AnyAsync(predicate);
        }


        public IEnumerable<Listening> GetAll()
        {
            return _db.Listening;
        }

        public IQueryable<Listening> GetAllWithExpressionsTree()
        {
            return _db.Listening;
        }

        public void Update(Listening item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public int Count(Expression<Func<Listening, Boolean>> predicate)
        {
            return _db.Listening.Count(predicate);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
