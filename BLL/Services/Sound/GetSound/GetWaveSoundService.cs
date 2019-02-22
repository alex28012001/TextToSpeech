using System.Collections.Generic;
using BLL.Dto;
using DAL.UnitOfWork;
using DAL.Entities;
using AutoMapper;
using System.Linq;
using System;
using DAL.Repository;
using System.Data.Entity;

namespace BLL.Services.Sound.GetSound
{
    public class GetWaveSoundService : IGetSoundService
    {
        private IUnitOfWork _db;
        public GetWaveSoundService(IUnitOfWork db)
        {
            _db = db;   
        }

        public IEnumerable<AudioDto> GetSubAudio(string userName, int offset, int count)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");

            IEnumerable<Audio> latestSubAudio = ((ISubRepository)_db.Subs)
                                                .FindWithExpressionTreeSubAudio(p => p.User.UserName.Equals(userName))   
                                                .OrderByDescending(p => p.Id)
                                                .Skip(offset)
                                                .Take(count);

            return GetMapperAudio(latestSubAudio);
        }

        public AudioDto FindAudio(string userName, string audioTitle)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            if (audioTitle == null)
                throw new ArgumentNullException("audioTitle");

           Audio audio = _db.Audio.FindWithExpressionsTree
                                    (p => p.User.UserName.Equals(userName) && p.Title.Equals(audioTitle))
                                    .FirstOrDefault();

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Audio, AudioDto>()
                .ForMember("UserName", src => src.MapFrom(p => p.User.UserName));
            }).CreateMapper();

            return mapper.Map<Audio, AudioDto>(audio);
        }

        public IEnumerable<AudioDto> FindAudio(string audioTitle, int offset, int count)
        {
            if (audioTitle == null)
                throw new ArgumentNullException("audioTitle");

            IEnumerable<Audio> audio = _db.Audio.FindWithExpressionsTree(p => p.Title.Contains(audioTitle))
                                                .OrderByDescending(p => p.Id)
                                                .Skip(offset)
                                                .Take(count);

            return GetMapperAudio(audio);
        }


        public IEnumerable<AudioDto> GetUserAudio(string userName, int offset, int count)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");

            IEnumerable<Audio> userAudio = _db.Audio.FindWithExpressionsTree(p => p.User.UserName.Equals(userName))
                                                    .OrderByDescending(p => p.Id)
                                                    .Skip(offset)
                                                    .Take(count);

            return GetMapperAudio(userAudio);
        }

        public IEnumerable<AudioDto> GetNewAudio(int offset, int count)
        {
            IEnumerable<Audio> newAudio = _db.Audio.GetAllWithExpressionsTree()
                                    .OrderByDescending(p => p.Id)
                                    .Skip(offset)
                                    .Take(count);

            return GetMapperAudio(newAudio);
        }

        public IEnumerable<AudioDto> GetPopularAudio(int offset, int count)
        {
            IEnumerable<Audio> popularAudio = _db.Audio.GetAllWithExpressionsTree()
                                                      .OrderByDescending(p => p.QuantityLikes)
                                                      .Skip(offset)
                                                      .Take(count);

            return GetMapperAudio(popularAudio);
        }

        public IEnumerable<AudioDto> GetLikedAudio(string userName, int offset, int count)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");

            IEnumerable<Audio> likedAudio = _db.Likes
                                         .FindWithExpressionsTree(p => p.User.UserName.Equals(userName))
                                         .OrderByDescending(p => p.Id)
                                         .Skip(offset)
                                         .Take(count)
                                         .Select(p => p.Audio)
                                         .Include("User");

            return GetMapperAudio(likedAudio);
        }


        private IEnumerable<AudioDto> GetMapperAudio(IEnumerable<Audio> audio)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Audio, AudioDto>().
                ForMember("UserName", src => src.MapFrom(p => p.User.UserName));
            }).CreateMapper();

            return mapper.Map<IEnumerable<Audio>, IEnumerable<AudioDto>>(audio); 
        }
    }
}
