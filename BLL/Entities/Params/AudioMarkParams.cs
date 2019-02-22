using System;

namespace BLL.Entities.Params
{
    public class AudioMarkParams : RelationParams
    { 
        private string _audioTitle;
        public AudioMarkParams(string yourName, string singerName, string audioTitle)
            :base(yourName,singerName)
        {
            AudioTitle = audioTitle;
        }

        public string AudioTitle
        {
            get { return _audioTitle; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("AudioTitle");
                _audioTitle = value;
            }
        }
    }
}
