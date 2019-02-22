using BLL.Dto;
using BLL.Entities.Params;
using BLL.Services.Comments;
using BLL.Services.Likes;
using BLL.Services.Subs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Voice.Controllers
{
    public class relationController : Controller
    {
        private ILikeService _likeService;
        private ISubService _subService;
        private ICommentService _commentService;

        public relationController(ILikeService likeService, ISubService subService, ICommentService commentService)
        {
            _likeService = likeService;
            _subService = subService;
            _commentService = commentService;
        }


        [HttpPost]
        public async Task AddComment(string text, string singerName, string audioTitle)
        {
            string userName = User.Identity.Name;
            CommentParams commentParams = new CommentParams(text, userName, singerName, audioTitle);
            await _commentService.AddCommentAsync(commentParams);
        }

        [HttpPost]
        public ActionResult FindComments(string singerName, string audioTitle)
        {
            FindCommentsParams fingCommentsParams = new FindCommentsParams(singerName, audioTitle);
            IEnumerable<CommentDto> comments = _commentService.FindComments(fingCommentsParams);

            return Json(comments);
        }


        [HttpPost]
        public async Task Follow(string singerName)
        {
            string userName = User.Identity.Name;
            RelationParams subParam = new RelationParams(userName, singerName);
            await _subService.FollowAsync(subParam);
        }

        [HttpPost]
        public async Task UnFollow(string singerName)
        {
            string userName = User.Identity.Name;
            RelationParams subParam = new RelationParams(userName, singerName);
            await _subService.UnFollowAsync(subParam);
        }

        [HttpPost]
        public async Task<ActionResult> IsSub(string singerName)
        {
            string userName = User.Identity.Name;
            RelationParams subParam = new RelationParams(userName, singerName);
            bool isSub = await _subService.IsSubAsync(subParam);

            return Content(isSub.ToString());
        }



        [HttpPost]
        public async Task AddListener(string singerName, string audioTitle)
        {
            string userName = User.Identity.Name;
            AudioMarkParams likeParams = new AudioMarkParams(userName, singerName, audioTitle);
            await _likeService.AddListeningAsync(likeParams);
        }


        [HttpPost]
        public async Task Like(string singerName, string audioTitle)
        {
            string userName = User.Identity.Name;
            AudioMarkParams likeParams = new AudioMarkParams(userName, singerName, audioTitle);
            await _likeService.LikeAsync(likeParams);
        }

        [HttpPost]
        public async Task Dislike(string singerName, string audioTitle)
        {
            string userName = User.Identity.Name;
            AudioMarkParams likeParams = new AudioMarkParams(userName, singerName, audioTitle);
            await _likeService.DislikeAsync(likeParams);
        }
    }
}