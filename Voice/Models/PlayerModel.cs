using BLL.Dto;

namespace Voice.Models
{
    public class PlayerModel
    {
        public AudioDto Audio { get; set; }
        public PlayerProperties PlayerProperties { get; set; }
        public int AmountProc { get; set; }
    }
}