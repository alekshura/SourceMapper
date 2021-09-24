using Compentio.SourceMapper.Metadata;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    interface IDependencyInjectionStrategy
    {
        IResult GenerateCode(ISourcesMetadata sourcesMetadata);
    }
}
