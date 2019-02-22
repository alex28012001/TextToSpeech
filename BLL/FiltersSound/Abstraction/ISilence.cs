using BLL.Entities.Params;

namespace BLL.FiltersSound.Abstraction
{
    public interface ISilence
    {
        void DurationSilence(byte[] sound, short minAmplitude, out int startPos, out int endPos);
        byte [] GetSoundWithoutSilence(SilencyParams param);
    }
}
