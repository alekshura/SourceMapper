using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Generators
{
    public class CodeSourceGeneratorTestBase
    {
        protected static GeneratorExecutionContext GetFakeContext(string sourceCode)
        {
            var compilation = CSharpCompilation.Create("CodeSourceGeneratorTests",
                                                       new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                                                       new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new FakeSourceGenerator();
            CSharpGeneratorDriver.Create(generator)
                                 .RunGeneratorsAndUpdateCompilation(compilation,
                                                                    out var outputCompilation,
                                                                    out var diagnostics);

            diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)
                       .Should().BeEmpty();

            return generator.GeneratorExecutionContext;
        }
    }
}