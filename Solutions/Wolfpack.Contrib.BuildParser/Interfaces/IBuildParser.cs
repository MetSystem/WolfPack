using Wolfpack.Contrib.BuildParser.Interfaces.Entities;

namespace Wolfpack.Contrib.BuildParser.Interfaces
{
    public interface IBuildParser
    {
        void Initialise(BuildParserConfig config);
    }
}