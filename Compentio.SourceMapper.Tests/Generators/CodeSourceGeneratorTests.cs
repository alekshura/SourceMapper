using AutoFixture;
using Compentio.SourceMapper.Generators;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Generators
{
    public class CodeSourceGeneratorTests : CodeSourceGeneratorTestBase
    {
        private readonly IFixture _fixture;
        private readonly Mock<DependencyInjection> _mockDependencyInjection;
        private readonly Mock<ISourcesMetadata> _mockSourcesMetadata;        

        public CodeSourceGeneratorTests()
        {
            _fixture = new Fixture();
            _mockDependencyInjection = _fixture.Create<Mock<DependencyInjection>>();
            _mockSourcesMetadata = _fixture.Create<Mock<ISourcesMetadata>>();
            _mockSourcesMetadata.Setup(s => s.DependencyInjection).Returns(_mockDependencyInjection.Object);
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
