using System;

namespace BLL.Entities.Params
{
    public class RelationParams
    {
        private string _yourName;
        private string _singerName;
        public RelationParams(string yourName, string singerName)
        {
            YourName = yourName;
            SingerName = singerName;
        }


        public string YourName
        {
            get { return _yourName; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("userName");
                _yourName = value;
            }
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
    }
}
