using Compentio.SourceMapper.Processors.DependencyInjection;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Collections.Generic;
using Compentio.SourceMapper.Diagnostics;

namespace Compentio.SourceMapper.Generators
{
    /// <summary>
    /// Class that setups code generation metadata and code processors.
    /// Mappers are generated for interfaces and abstract classes.
    /// </summary>
    internal class CodeSourceGenerator
    {
        private readonly ISourcesMetadata _sourcesMetadata;

        internal CodeSourceGenerator (ISourcesMetadata sourcesMetadata)
        {
            _sourcesMetadata = sourcesMetadata;
        }

        internal void GenerateMappings(GeneratorExecutionContext context)
        {
            foreach (var mapper in _sourcesMetadata.Mappers)
            {
                var processorStrategy = ProcessorStrategyFactory.GetStrategy(mapper);

                var mapperCode = processorStrategy.GenerateCode(mapper);
                
                ReportDiagnostics(context, processorStrategy.Diagnostics);

                context.AddSource(mapper.FileName, SourceText.From(mapperCode, Encoding.UTF8));
            }
        }

        internal void GenerateDependencyInjectionExtensions(GeneratorExecutionContext context)
        {
            var processorStrategy = DependencyInjectionStrategyFactory.GetStrategy(_sourcesMetadata);

            var extensionCode = processorStrategy.GenerateCode(_sourcesMetadata);

            context.AddSource($"{_sourcesMetadata.DependencyInjection.DependencyInjectionClassName}.cs", SourceText.From(extensionCode, Encoding.UTF8));
        }

        private void ReportDiagnostics(GeneratorExecutionContext context, IEnumerable<DiagnosticsInfo> diagnostics)
        {
            foreach (var diagnosticInfo in diagnostics)
            {
                context.ReportDiagnostic(Diagnostic.Create(diagnosticInfo.DiagnosticDescriptor, diagnosticInfo.Metadata.Location, diagnosticInfo.Metadata.Name));
            }            
        }
    }
}
