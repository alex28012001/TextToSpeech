using System;

namespace BLL.Entities.Params
{
    public class CommentParams : AudioMarkParams
    {
        private string _text;
      
        public CommentParams(string text, string userName, string singerName, string audioTitle) 
            : base(userName, singerName, audioTitle)
        {
            Text = text;
            AudioTitle = audioTitle;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("YourName");
                _text = value;
            }
        }
    }
}
