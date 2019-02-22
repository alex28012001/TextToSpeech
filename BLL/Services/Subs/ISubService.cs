using BLL.Entities.Params;
using System.Threading.Tasks;

namespace BLL.Services.Subs
{
    public interface ISubService
    {
        Task FollowAsync(RelationParams param);
        Task UnFollowAsync(RelationParams param);
        Task<bool> IsSubAsync(RelationParams param);
    }
}
