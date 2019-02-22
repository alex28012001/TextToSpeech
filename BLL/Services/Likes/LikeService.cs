using BLL.Entities.Params;
using DAL.Entities;
using DAL.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.Likes
{
    public class LikeService : ILikeService
    {
        private IUnitOfWork _db;
        public LikeService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task LikeAsync(AudioMarkParams model)
        {
            ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(model.YourName)).FirstOrDefault();
            Audio audio = _db.Audio.FindWithExpressionsTree(p => p.User.UserName.Equals(model.SingerName) && p.Title.Equals(model.AudioTitle)).FirstOrDefault();

            if (user != null && audio != null)
            {
                Like like = new DAL.Entities.Like() { User = user, Audio = audio };
                _db.Likes.Create(like);
                ++audio.QuantityLikes;
                await _db.SaveAsync();
            }
        }

        public async Task DislikeAsync(AudioMarkParams model)
        {
            Like like = _db.Likes.FindWithExpressionsTree(
                              p => p.User.UserName.Equals(model.YourName) &&
                              p.Audio.User.UserName.Equals(model.SingerName) &&
                              p.Audio.Title.Equals(model.AudioTitle)).FirstOrDefault();

            if (like != null)
            {
                --like.Audio.QuantityLikes;
                _db.Likes.Remove(like);
                await _db.SaveAsync();
            }
        }


        public async Task<bool> IsLikedAsync(AudioMarkParams model)
        {
            return await _db.Likes.AnyAsync(
                               p => p.User.UserName.Equals(model.YourName) &&
                               p.Audio.User.UserName.Equals(model.SingerName) &&
                               p.Audio.Title.Equals(model.AudioTitle));
        }

        public async Task AddListeningAsync(AudioMarkParams model)
        {
            bool haveYourListening = await _db.Listening.AnyAsync(
                                         p => p.User.UserName.Equals(model.YourName) &&
                                         p.Audio.User.UserName.Equals(model.SingerName) &&
                                         p.Audio.Title.Equals(model.AudioTitle));

            if (!haveYourListening)
            {
                ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(model.YourName)).FirstOrDefault();
                Audio audio = _db.Audio.FindWithExpressionsTree(p => p.User.UserName.Equals(model.SingerName) && p.Title.Equals(model.AudioTitle)).FirstOrDefault();

                if (user != null && audio != null)
                {
                    Listening listening = new Listening() { User = user, Audio = audio };
                    _db.Listening.Create(listening);
                    audio.Listening++;
                    await _db.SaveAsync();
                }
            }
        }
    }
}

