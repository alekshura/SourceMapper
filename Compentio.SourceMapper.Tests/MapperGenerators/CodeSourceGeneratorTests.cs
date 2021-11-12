using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Generators;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Generators
{
    public class CodeSourceGeneratorTests : CodeSourceGeneratorTestBase
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMapperMetadata> _mockMapperMetadata;
        private readonly Mock<ISourcesMetadata> _mockSourcesMetadata;

        private readonly CodeSourceGenerator _codeSourceGenerator;
        private readonly GeneratorExecutionContext _generatorExecutionContext;

        public CodeSourceGeneratorTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization()); 

            _mockMapperMetadata = _fixture.Create<Mock<IMapperMetadata>>();
            _mockSourcesMetadata = _fixture.Create<Mock<ISourcesMetadata>>();
            _mockSourcesMetadata.Setup(s => s.Mappers).Returns(new List<IMapperMetadata> { _mockMapperMetadata.Object });

            _codeSourceGenerator = new CodeSourceGenerator(_mockSourcesMetadata.Object);
            _generatorExecutionContext = GetFakeContext(GetTestCode());
        }

        [Fact]
        public void GenerateDependencyInjectionExtensions_StrategyNone_ReportDiagnostics()
        {
            // Arrange
            _mockSourcesMetadata.Setup(s => s.DependencyInjection.DependencyInjectionType).Returns(DependencyInjectionType.None);

            // Act
            _codeSourceGenerator.GenerateDependencyInjectionExtensions(_generatorExecutionContext);
            var diagnostics = _generatorExecutionContext.Compilation.GetDiagnostics();

            // Assert
            diagnostics.Should().NotBeNull();
            diagnostics.Should().NotBeEmpty();
            diagnostics.Should().OnlyContain(x => x.Id == SourceMapperDescriptors.DependencyInjectionNotUsed.Id);
        }

        [Fact]
        public void GenerateDependencyInjectionExtensions_DotNetCoreStrategy_EmptyDiagnostics()
        {
            // Arrange
            _mockSourcesMetadata.Setup(s => s.DependencyInjection.DependencyInjectionType).Returns(DependencyInjectionType.DotNetCore);

            // Act
            _codeSourceGenerator.GenerateDependencyInjectionExtensions(_generatorExecutionContext);
            var diagnostics = _generatorExecutionContext.Compilation.GetDiagnostics();

            // Assert
            diagnostics.Should().NotBeNull();
            diagnostics.Should().BeEmpty();
        }

        [Fact]
        public void GenerateDependencyInjectionExtensions_AutofacStrategy_EmptyDiagnostics()
        {
            // Arrange
            _mockSourcesMetadata.Setup(s => s.DependencyInjection.DependencyInjectionType).Returns(DependencyInjectionType.Autofac);

            // Act
            _codeSourceGenerator.GenerateDependencyInjectionExtensions(_generatorExecutionContext);
            var diagnostics = _generatorExecutionContext.Compilation.GetDiagnostics();

            // Assert
            diagnostics.Should().NotBeNull();
            diagnostics.Should().BeEmpty();
        }

        [Fact]
        public void GenerateMapping_NoMappers_EmptyDiagnostics()
        {
            // Arrange
            _mockSourcesMetadata.Setup(s => s.Mappers).Returns(new List<IMapperMetadata>());

            // Act
            _codeSourceGenerator.GenerateMappings(_generatorExecutionContext);
            var diagnostics = _generatorExecutionContext.Compilation.GetDiagnostics();

            // Assert
            diagnostics.Should().NotBeNull();
            diagnostics.Should().BeEmpty();
        }

        [Fact]
        public void GenerateMapping_MappersWithoutMethods_EmptyDiagnostics()
        {
            // Arrange
            _mockMapperMetadata.Setup(m => m.MethodsMetadata).Returns(new List<IMethodMetadata>());

            // Act
            _codeSourceGenerator.GenerateMappings(_generatorExecutionContext);
            var diagnostics = _generatorExecutionContext.Compilation.GetDiagnostics();

            // Assert
            diagnostics.Should().NotBeNull();
            diagnostics.Should().BeEmpty();
        }

        private static string GetTestCode()
        {
            return @"
namespace FakeCodeGeneratorNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
";
        }
    }
}