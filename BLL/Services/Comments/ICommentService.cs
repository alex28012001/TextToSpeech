using BLL.Dto;
using BLL.Entities.Params;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Comments
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentParams param);
        IEnumerable<CommentDto> FindComments(FindCommentsParams param);
    }
}
