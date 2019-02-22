using BLL.Entities.Params.BuildSoundParam;
using BLL.Infastructure;
using System.Collections.Generic;

namespace BLL.BuildSound.Reader
{
    public interface IReaderText
    {
        ResultDetails<IEnumerable<string>> Read(ReaderParams param);
    }
}
