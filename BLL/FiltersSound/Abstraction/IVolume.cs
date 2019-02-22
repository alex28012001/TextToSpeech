namespace BLL.FiltersSound.Abstraction
{
    public interface IVolume
    {
         byte[] SetVolume(byte[] soundBytes, int percent);
    }
}
