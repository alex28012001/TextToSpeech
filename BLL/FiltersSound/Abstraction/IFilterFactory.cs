namespace BLL.FiltersSound.Abstraction
{
    public interface IFilterFactory
    {
        IChangerAmplitude CreateChangerAmplitude();
        IChangerVoice CreateChangerVoice(int sampleRate,int channels);
        IVolume CreateVolume();
    }
}
