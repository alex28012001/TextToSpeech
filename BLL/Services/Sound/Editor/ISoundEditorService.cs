using BLL.Entities.Params.SavedSoundParam;
using BLL.Infastructure;
using System.Threading.Tasks;

namespace BLL.Services.Sound.Editor
{
    public interface ISoundEditorService
    {
        Task<OperationDetails> SaveAudioByStreamAsync(SaveAudioByStreamParams param);
        Task<OperationDetails> SaveAudioByTextAsync(SaveAudioByTextParams param);
        void DeleteAudio(string userName, string audioTitle);
    }
}
