using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DAL.UnitOfWork;
using DAL.Context;
using System.Data.Entity;

namespace DAL.Repository
{
    public class WordAccentRepository : IRepository<WordAccent>
    {
        private VoiceContext _db;
        public WordAccentRepository(DbContext db)
        {
            _db = (VoiceContext)db;
        }

        public void Create(WordAccent item)
        {
            _db.WordAccents.Add(item);
        }

        public void Remove(WordAccent item)
        {
            _db.WordAccents.Remove(item);
        }

        public IEnumerable<WordAccent> Find(Func<WordAccent, bool> predicate)
        {
            return _db.WordAccents.Where(predicate);
        }

        public IQueryable<WordAccent> FindWithExpressionsTree(Expression<Func<WordAccent, bool>> predicate)
        {
            return _db.WordAccents.Where(predicate);
        }

        public bool Any(Expression<Func<WordAccent, bool>> predicate)
        {
            return _db.WordAccents.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<WordAccent, bool>> predicate)
        {
            return await _db.WordAccents.AnyAsync(predicate);
        }

        public int Count(Expression<Func<WordAccent, bool>> predicate)
        {
            return _db.WordAccents.Count(predicate);
        }

        public IEnumerable<WordAccent> GetAll()
        {
            return _db.WordAccents;
        }

        public IQueryable<WordAccent> GetAllWithExpressionsTree()
        {
            return _db.WordAccents;
        }

        public void Update(WordAccent item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
