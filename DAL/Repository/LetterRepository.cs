using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Dto;

namespace DAL.Repository
{
    public class LetterRepository : IRepository<Letter>
    {
        private VoiceContext _db;
        public LetterRepository(DbContext db)
        {
            _db = db as VoiceContext;
        }

        public void Create(Letter item)
        {
            _db.Letters.Add(item);
        }

        public void Remove(Letter item)
        {
            _db.Letters.Remove(item);
        }

        public IEnumerable<Letter> Find(Func<Letter, bool> predicate)
        {
            return _db.Letters.Where(predicate);
        }

        public IQueryable<Letter> FindWithExpressionsTree(Expression<Func<Letter, Boolean>> predicate)
        {
            return _db.Letters.Where(predicate);             
        }

        public void Update(Letter item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Letter> GetAll()
        {
            return _db.Letters.Select(p => new
                               {
                                   LetterName = p.LetterName,
                                   Src = p.Src,
                                   UserName = p.User.UserName
                               }).AsEnumerable()
                               .Select(p => new Letter()
                               {
                                   LetterName = p.LetterName,
                                   Src = p.Src,
                                   User = new ClientProfile() { UserName = p.UserName}
                               });
        }

        public IQueryable<Letter> GetAllWithExpressionsTree()
        {
            return _db.Letters;
        }


        public bool Any(Expression<Func<Letter, bool>> predicate)
        {
            return _db.Letters.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<Letter, bool>> predicate)
        {
            return await _db.Letters.AnyAsync(predicate);
        }

        public int Count(Expression<Func<Letter, Boolean>> predicate)
        {
            return _db.Letters.Count(predicate); 
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
