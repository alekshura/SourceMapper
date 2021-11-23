using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Moq;
using System.Linq;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.Processors
{
    public abstract class ProcessorStrategyTestBase
    {
        protected readonly IFixture _fixture;
        protected readonly Mock<MappingAttribute> _mockMappingAttribute;

        protected ProcessorStrategyTestBase()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockMappingAttribute = _fixture.Create<Mock<MappingAttribute>>();
        }

        protected string GetGeneratedOutput(string sourceCode)
        {
            var compilation = CSharpCompilation.Create("MainSourceGeneratorTests",
                                                       new[] { CSharpSyntaxTree.ParseText(sourceCode) },
                                                       new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                                                       new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new MainSourceGenerator();
            CSharpGeneratorDriver.Create(generator).RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _);

            return outputCompilation.SyntaxTrees.Skip(1).LastOrDefault()?.ToString();
        }
    }
}