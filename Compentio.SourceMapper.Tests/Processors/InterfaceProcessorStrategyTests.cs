using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class InterfaceProcessorStrategyTests
    {
        private readonly IFixture _fixture;
        private readonly IProcessorStrategy _processorStrategy;
        private readonly Mock<ISourceMetadata> _sourceMetadataMock;

        public InterfaceProcessorStrategyTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());

            _sourceMetadataMock = _fixture.Create<Mock<ISourceMetadata>>();
            _processorStrategy = ProcessorStrategyFactory.GetStrategy(TypeKind.Interface);
            
        }

        [Fact]
        public void GenerateCode_Returns_Code()
        {
            // Arrange 
            var mathodsMetadata = _fixture.CreateMany<IMethodMetadata>();

            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MapperName).Returns("MapperName");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.Namespace).Returns("Namespace");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TargetClassName).Returns("TargetClassName");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(mathodsMetadata);

            _processorStrategy.Initialize(_sourceMetadataMock.Object);

            // Act
            var generatedCode = _processorStrategy.GenerateCode();

            // Assert
            generatedCode.Should().NotBeNullOrEmpty();
            
        }
    }
}
