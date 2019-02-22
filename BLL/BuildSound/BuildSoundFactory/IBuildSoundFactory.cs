using BLL.BuildSound.Accent;
using BLL.BuildSound.ParsingText;
using BLL.BuildSound.Reader;
using BLL.Concatenation;

namespace BLL.BuildSound.BuildSoundFactory
{
    public interface IBuildSoundFactory
    {
        IWordAccent CreateWordAccent();
        IReaderText CreateReaderText();
        IConcatenation CreateConcatenation();
        IParserText CreateParserText();
    }
}
