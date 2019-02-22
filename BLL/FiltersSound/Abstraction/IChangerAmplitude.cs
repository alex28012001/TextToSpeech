namespace BLL.FiltersSound.Abstraction
{
    public interface IChangerAmplitude
    {
        byte[] SkipSilence(byte[] sound, short minAmplitude);
        byte[] MakeSoundFlowing(byte[] sound);
    }
}
