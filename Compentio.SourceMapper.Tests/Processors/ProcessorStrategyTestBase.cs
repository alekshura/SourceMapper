using Compentio.SourceMapper.Generators;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ProcessorStrategyTestBase
    {
        protected static string GetGeneratedOutput(string sourceCode)
        {
            var compilation = CSharpCompilation.Create("MainSourceGeneratorTests",
                                                       new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                                                       new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new MainSourceGenerator();
            CSharpGeneratorDriver.Create(generator)
                                 .RunGeneratorsAndUpdateCompilation(compilation,
                                                                    out var outputCompilation,
                                                                    out var diagnostics);

            diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)
                       .Should().BeEmpty();

            return outputCompilation.SyntaxTrees.Skip(1).LastOrDefault()?.ToString();
        }
    }
}
