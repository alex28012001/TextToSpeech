using BLL.Entities.Params.BuildSoundParam;
using BLL.Infastructure;
using System.IO;

namespace BLL.Services.Sound.BuildSound
{
    public interface IBuilderSoundService
    {
        ResultDetails<Stream> BuildSound(BuildSoundParams param);
    }
}
