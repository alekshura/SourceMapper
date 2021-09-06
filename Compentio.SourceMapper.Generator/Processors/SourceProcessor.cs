namespace Compentio.SourceMapper.Processors
{
    public interface ISourceProcessor
    {
        string FileName { get; }
        string GenerateCode();           
    }
}
