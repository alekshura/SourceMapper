namespace Compentio.SourceMapper.Processors
{
    interface ISourceProcessor
    {
        string FileName { get; }
        string GenerateCode();           
    }
}
