using System.ComponentModel.DataAnnotations;

namespace Voice.Models
{
    public class CreateModel
    {
        [Required(ErrorMessage = "текст должен содержать хотя-бы 1 алфавитно-числовой символ")]
        [Display(Name = "Текст")]
        public string Text { get; set; }
        public string Title { get; set; }
        public int SampleRate { get; set; }
    }
}