using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.MapperGenerators
{
    public class CodeSourceGeneratorTestBase
    {
        protected GeneratorExecutionContext GetFakeContext(string sourceCode)
        {
            var compilation = CSharpCompilation.Create("CodeSourceGeneratorTests",
                                                       new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                                                       new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new FakeSourceGenerator();
            CSharpGeneratorDriver.Create(generator).RunGeneratorsAndUpdateCompilation(compilation, out _, out _);

            return generator.GeneratorExecutionContext;
        }
    }
}