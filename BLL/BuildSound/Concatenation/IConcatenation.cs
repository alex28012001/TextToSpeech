using System.Collections.Generic;
using System.IO;

namespace BLL.Concatenation
{
    public interface IConcatenation
    {
        byte[] Concatenate(IEnumerable<string> soundSrcs);
    }
}
