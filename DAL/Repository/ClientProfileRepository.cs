
using DAL.Context;
using DAL.Dto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ClientProfileRepository : IRepository<ClientProfile>
    {
        private VoiceContext _db;
        public ClientProfileRepository(DbContext db)
        {
            _db = db as VoiceContext;
        }
        public void Create(ClientProfile item)
        {
            _db.Clientprofiles.Add(item);
        }

        public void Remove(ClientProfile item)
        {
            _db.Clientprofiles.Remove(item);
        }

        public void Update(ClientProfile item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<ClientProfile> Find(Func<ClientProfile, bool> predicate)
        {
            return _db.Clientprofiles.Where(predicate);
        }

        public IQueryable<ClientProfile> FindWithExpressionsTree(Expression<Func<ClientProfile, Boolean>> predicate)
        {
            return _db.Clientprofiles.Where(predicate);
        }


        public IEnumerable<ClientProfile> GetAll()
        {
            return _db.Clientprofiles;                                         
        }

        public IQueryable<ClientProfile> GetAllWithExpressionsTree()
        {
            return _db.Clientprofiles;
        }

        public async Task<bool> AnyAsync(Expression<Func<ClientProfile, Boolean>> predicate)
        {
            return await _db.Clientprofiles.AnyAsync(predicate);
        }

        public bool Any(Expression<Func<ClientProfile, Boolean>> predicate)
        {
            return _db.Clientprofiles.Any(predicate);
        }

        public int Count(Expression<Func<ClientProfile, Boolean>> predicate)
        {
            return _db.Clientprofiles.Count(predicate);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
