using System.IO;
using BLL.Entities.Params.BuildSoundParam;
using BLL.Infastructure;
using BLL.BuildSound.Builder;

namespace BLL.Services.Sound.BuildSound
{
    public class BuilderWaveSoundService : IBuilderSoundService
    {
        private BuilderDirector _builderDirector;
        public BuilderWaveSoundService(ISoundBuilder soundBuilder)
        {
            _builderDirector = new BuilderDirector(soundBuilder);
        }

        public ResultDetails<Stream> BuildSound(BuildSoundParams param)
        {
            return _builderDirector.BuildSound(param);
        }
    }
}
