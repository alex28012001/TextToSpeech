using BLL.Entities.Params;
using DAL.Entities;
using DAL.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.Subs
{
    public class SubService : ISubService
    {
        private IUnitOfWork _db;
        public SubService(IUnitOfWork db)
        {
            _db = db;
        }


        public async Task FollowAsync(RelationParams param)
        {
            ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(param.YourName)).FirstOrDefault();
            ClientProfile singer = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(param.SingerName)).FirstOrDefault();

            if (user != null && singer != null)
            {
                Sub sub = new Sub() { User = user, Singer = singer };
                _db.Subs.Create(sub);
                ++user.AmountSubs;
                await _db.SaveAsync();
            }
        }

        public async Task UnFollowAsync(RelationParams param)
        {
            Sub sub = _db.Subs.FindWithExpressionsTree(p => p.User.UserName.Equals(param.YourName) && p.Singer.UserName.Equals(param.SingerName))
                              .FirstOrDefault();

            if (sub != null)
            {
                ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(param.YourName)).FirstOrDefault();
                _db.Subs.Remove(sub);
                --user.AmountSubs;
                await _db.SaveAsync();
            }
        }

        public async Task<bool> IsSubAsync(RelationParams param)
        {
            return await _db.Subs.AnyAsync
                (p => p.User.UserName.Equals(param.YourName) && p.Singer.UserName.Equals(param.SingerName));
        }
    }
}
