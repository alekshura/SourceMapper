namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        string GenerateCode(ISourceMetadata sourceMetadata);           
    }
}
