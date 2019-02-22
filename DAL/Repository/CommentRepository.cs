using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DAL.Context;
using System.Data.Entity;

namespace DAL.Repository
{
    public class CommentRepository : IRepository<Comment>
    {
        private VoiceContext _db;
        public CommentRepository(DbContext db)
        {
            _db = (VoiceContext)db;
        }

        public void Create(Comment item)
        {
            _db.Comments.Add(item);
        }

        public void Remove(Comment item)
        {
            _db.Comments.Remove(item);
        }

        public IEnumerable<Comment> Find(Func<Comment, bool> predicate)
        {
            return _db.Comments.Include("User").Where(predicate);
        }

        public IQueryable<Comment> FindWithExpressionsTree(Expression<Func<Comment, bool>> predicate)
        {
            return _db.Comments.Include("User").Where(predicate);
        }

        public bool Any(Expression<Func<Comment, bool>> predicate)
        {
            return _db.Comments.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await _db.Comments.AnyAsync(predicate);
        }

        public int Count(Expression<Func<Comment, bool>> predicate)
        {
            return _db.Comments.Count(predicate);
        }


        public IEnumerable<Comment> GetAll()
        {
            return _db.Comments.Include("User").Include("Audio");
        }

        public IQueryable<Comment> GetAllWithExpressionsTree()
        {
            return _db.Comments.Include("User").Include("Audio");
        }


        public void Update(Comment item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
