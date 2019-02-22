using BLL.BuildSound.ParsingText;
using BLL.Concatenation;
using BLL.BuildSound.Reader;
using DAL.UnitOfWork;
using BLL.BuildSound.Accent;
using BLL.FiltersSound.Implementation;

namespace BLL.BuildSound.BuildSoundFactory
{
    public class BuildSoundWaveRusFactory : IBuildSoundFactory
    {
        private IUnitOfWork _db;
        public BuildSoundWaveRusFactory(IUnitOfWork db)
        {
            _db = db;
        }

        public IWordAccent CreateWordAccent()
        {
            return new RusWordAccent(_db);
        }
        public IParserText CreateParserText()
        {
            return new ParserRusText();
        }
        public IReaderText CreateReaderText()
        {
            return new ReaderRusText(_db);
        }
        public IConcatenation CreateConcatenation()
        {
            return new ConcatenationWaveFiles(new WaveVolume());
        }      
    }
}
