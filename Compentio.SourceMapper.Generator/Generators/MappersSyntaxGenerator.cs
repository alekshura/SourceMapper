using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Compentio.SourceMapper.Generators
{
    /// <summary>
    /// Class that setups code generation metadata and code processors.
    /// Mappers are generated for interfaces and abstract classes.
    /// </summary>
    internal class MappersSyntaxGenerator
    {
        private readonly ISourcesMetadata _sourcesMetadata;

        internal MappersSyntaxGenerator (ISourcesMetadata sourcesMetadata)
        {
            _sourcesMetadata = sourcesMetadata;
        }

        internal void Execute(GeneratorExecutionContext context)
        {
            foreach (var mapper in _sourcesMetadata.Mappers)
            {
                var processorStrategy = ProcessorStrategyFactory.GetStrategy(mapper);

                var mapperCode = processorStrategy.GenerateCode(mapper);


                context.AddSource(mapper.FileName, SourceText.From(mapperCode, Encoding.UTF8));
            }
        }             
    }
}
