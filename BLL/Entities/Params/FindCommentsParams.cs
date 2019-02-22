using System;

namespace BLL.Entities.Params
{
    public class FindCommentsParams
    {
        private string _singerName;
        private string _audioTitle;
        public FindCommentsParams(string singerName, string audioTitle)
        {
            SingerName = singerName;
            AudioTitle = audioTitle;
        }

        public string SingerName
        {
            get { return _singerName; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("singerName");
                _singerName = value;
            }
        }

        public string AudioTitle
        {
            get { return _audioTitle; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("AudioTitle");
                _audioTitle = value;
            }
        }
    }
}
