using BLL.Entities;
using BLL.Entities.Params.SavedSoundParam;
using BLL.Infastructure;
using System.Threading.Tasks;

namespace BLL.Services.Letters
{
    public interface ILetterService
    {
        Task<OperationDetails> SaveLetterAsync(SaveLetterParams param);
        Task<bool> IsLetterRecordedAsync(string userName, string letterName);
        bool IsVoiceRecoreded(string userName, Language language);
    }
}
