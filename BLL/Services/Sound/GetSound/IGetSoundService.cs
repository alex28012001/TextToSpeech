using BLL.Dto;
using System.Collections.Generic;

namespace BLL.Services.Sound.GetSound
{
    public interface IGetSoundService
    {
        IEnumerable<AudioDto> GetSubAudio(string userName, int offset, int count);
        AudioDto FindAudio(string userName, string audioTitle);
        IEnumerable<AudioDto> FindAudio(string audioTitle, int offset, int count);
        IEnumerable<AudioDto> GetUserAudio(string userName, int offset, int count);
        IEnumerable<AudioDto> GetNewAudio(int offset, int count);
        IEnumerable<AudioDto> GetPopularAudio(int offset, int count);
        IEnumerable<AudioDto> GetLikedAudio(string userName, int offset, int count);
    }
}
