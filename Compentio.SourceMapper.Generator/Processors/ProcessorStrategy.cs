using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        string GenerateCode(ISourceMetadata sourceMetadata);           
    }
}
