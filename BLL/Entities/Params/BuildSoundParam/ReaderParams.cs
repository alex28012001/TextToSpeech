using System;

namespace BLL.Entities.Params.BuildSoundParam
{
    public class ReaderParams
    {
        private string _text;
        private string _userName;

        public ReaderParams(string text, string userName, Language language)
        {
            Text = text;
            UserName = userName;
            Language = language;
        }

        public Language Language { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Text", "field text can not be null");
                _text = value;
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("UserName", "field UserName can not be empty");
                _userName = value;
            }
        }
    }
}
