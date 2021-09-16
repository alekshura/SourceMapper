using System;
using Xunit;
using Compentio.SourceMapper.Processors;
using Microsoft.CodeAnalysis;
using FluentAssertions;
using Moq;
using Compentio.SourceMapper.Metadata;
using AutoFixture;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ProcessorStrategyFactoryTests
    {
        private readonly Mock<ISourceMetadata> _sourceMetadataMock;
        private readonly IFixture _fixture;

        public ProcessorStrategyFactoryTests()
        {
            _fixture = new Fixture();
            _sourceMetadataMock = _fixture.Create<Mock<ISourceMetadata>>();
        }

        [Fact]
        public void GetStrategy_Returns_InterfaceProcessorStrategy()
        {
            // Arrange
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TypeKind).Returns(TypeKind.Interface);

            // Act
            var processorStrategy = ProcessorStrategyFactory.GetStrategy(_sourceMetadataMock.Object);

            // Assert
            processorStrategy.Should().NotBeNull();
            processorStrategy.Should().BeOfType<InterfaceProcessorStrategy>();
        }

        [Fact]
        public void GetStrategy_For_AnyType_Returns_InterfaceProcessorStrategy()
        {
            // Arrange
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TypeKind).Returns(TypeKind.Dynamic);

            // Act
            var processorStrategy = ProcessorStrategyFactory.GetStrategy(_sourceMetadataMock.Object);

            // Assert
            processorStrategy.Should().NotBeNull();
            processorStrategy.Should().BeOfType<InterfaceProcessorStrategy>();
        }

        [Fact]
        public void GetStrategy_Returns_ClassProcessorStrategy()
        {
            // Arrange
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TypeKind).Returns(TypeKind.Class);

            // Act
            var processorStrategy = ProcessorStrategyFactory.GetStrategy(_sourceMetadataMock.Object);

            // Assert
            processorStrategy.Should().NotBeNull();
            processorStrategy.Should().BeOfType<ClassProcessorStrategy>();
        }
    }
}
