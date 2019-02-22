using BLL.Entities.Params;
using System.Threading.Tasks;

namespace BLL.Services.Likes
{
    public interface ILikeService
    {
        Task LikeAsync(AudioMarkParams model);
        Task DislikeAsync(AudioMarkParams model);
        Task<bool> IsLikedAsync(AudioMarkParams model);
        Task AddListeningAsync(AudioMarkParams model);  
    }
}
