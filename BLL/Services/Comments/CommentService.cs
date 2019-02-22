using DAL.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities.Params;
using DAL.Entities;
using BLL.Dto;
using System.Collections.Generic;
using AutoMapper;

namespace BLL.Services.Comments
{
    public class CommentService : ICommentService
    {
        private IUnitOfWork _db;
        public CommentService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task AddCommentAsync(CommentParams param)
        {
            ClientProfile user = _db.ClientProfiles.FindWithExpressionsTree(p => p.UserName.Equals(param.YourName))
                                                   .FirstOrDefault();
            Audio audio = _db.Audio.FindWithExpressionsTree(p => p.User.UserName.Equals(param.SingerName) && p.Title.Equals(param.AudioTitle))
                                   .FirstOrDefault();

            if(user != null && audio != null)
            {
                audio.QuantityComments++;

                DateTime dataCreating = DateTime.UtcNow;
                Comment comment = new Comment() { Text = param.Text, User = user, Audio = audio, Date = dataCreating };
                _db.Comments.Create(comment);

                await _db.SaveAsync();
            }
        }

        public IEnumerable<CommentDto> FindComments(FindCommentsParams param)
        {
            IEnumerable<Comment> foundedComments =
                _db.Comments.FindWithExpressionsTree(p => p.Audio.User.UserName.Equals(param.SingerName)
                                                          && p.Audio.Title.Equals(param.AudioTitle));

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Comment, CommentDto>()
                .ForMember("UserName", src => src.MapFrom(p => p.User.UserName));
            }).CreateMapper();

            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(foundedComments);  
        }
    }
}
