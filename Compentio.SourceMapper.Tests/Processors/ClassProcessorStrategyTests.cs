using AutoFixture;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ClassProcessorStrategyTests : ProcessorStrategyTestBase
    {
        private readonly IProcessorStrategy _processorStrategy;
        private readonly Mock<IMapperMetadata> _sourceMetadataMock;

        public ClassProcessorStrategyTests()
        {
            _sourceMetadataMock = _fixture.Create<Mock<IMapperMetadata>>();
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.Name).Returns("MapperName");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.Namespace).Returns("Namespace");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TargetClassName).Returns("TargetClassName");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TypeKind).Returns(TypeKind.Class);

            _processorStrategy = ProcessorStrategyFactory.GetStrategy(_sourceMetadataMock.Object);
        }

        [Fact]
        public void GenerateCode_Returns_NotEmpty_Code()
        {
            // Arrange
            var mathodsMetadata = _fixture.CreateMany<IMethodMetadata>();

            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(mathodsMetadata);

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Assert
            result.GeneratedCode.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateCode_Check_Diagnostics()
        {
            // Arrange
            var mathodsMetadata = _fixture.CreateMany<IMethodMetadata>();

            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(mathodsMetadata);

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);
            var generatorResult = GetGeneratedOutput(result.GeneratedCode);

            // Assert
            result.GeneratedCode.Should().NotBeNullOrEmpty();
        }
    }
}