namespace BLL.FiltersSound.Abstraction
{
    public interface IChangerVoice
    {
        float Temp { get; set; }
        float Pitch { get; set; }
        byte[] Change(byte[] soundBytes);
    }
}
