using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Extensions;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using Compentio.SourceMapper.Processors.DependencyInjection;
using Compentio.SourceMapper.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Generators
{
    /// <summary>
    /// Class that setups code generation metadata and code processors.
    /// Mappers are generated for interfaces and abstract classes.
    /// </summary>
    internal class CodeSourceGenerator
    {
        private readonly ISourcesMetadata _sourcesMetadata;
        private readonly List<DiagnosticsInfo> _diagnostics = new();

        internal CodeSourceGenerator(ISourcesMetadata sourcesMetadata)
        {
            _sourcesMetadata = sourcesMetadata;
        }

        internal void GenerateMappings(GeneratorExecutionContext context)
        {
            foreach (var mapper in _sourcesMetadata.Mappers.Where(m => !m.IsReferenced))
            {
                var processorStrategy = ProcessorStrategyFactory.GetStrategy(mapper);

                var result = processorStrategy.GenerateCode(mapper);

                ReportDiagnostics(context, result.Diagnostics);

                if (result.IsSuccess)
                {
                    context.AddSource(mapper.FileName, SourceText.From(result.GeneratedCode, Encoding.UTF8));
                }
            }
        }

        internal void GenerateDependencyInjectionExtensions(GeneratorExecutionContext context)
        {
            var processorStrategy = DependencyInjectionStrategyFactory.GetStrategy(_sourcesMetadata);

            if (processorStrategy is null)
            {
                AddDependencyInjectionDiagnostic(context);
                ReportDiagnostics(context, _diagnostics);
                return;
            }

            if (!_sourcesMetadata.Mappers.Any()) return;

            var result = processorStrategy.GenerateCode(_sourcesMetadata);

            context.AddSource($"{_sourcesMetadata.DependencyInjection.DependencyInjectionClassName}.cs", SourceText.From(result.GeneratedCode, Encoding.UTF8));
        }

        private void AddDependencyInjectionDiagnostic(GeneratorExecutionContext context)
        {
            _diagnostics.Add(new DiagnosticsInfo()
            {
                DiagnosticDescriptor = SourceMapperDescriptors.DependencyInjectionNotUsed,
                Exception = new DependencyInjectionException(AppStrings.DependencyInjectionNotUsedException, context.Compilation.AssemblyName)
            });
        }

        private void ReportDiagnostics(GeneratorExecutionContext context, IEnumerable<DiagnosticsInfo> diagnostics)
        {
            foreach (var diagnosticInfo in diagnostics)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    diagnosticInfo.DiagnosticDescriptor,
                    diagnosticInfo.Metadata != null ? Location.None : diagnosticInfo.Metadata?.Location,
                    diagnosticInfo.Message));
            }
        }
    }
}