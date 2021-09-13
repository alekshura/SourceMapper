using System;
using Xunit;
using Compentio.SourceMapper.Processors;
using Microsoft.CodeAnalysis;
using FluentAssertions;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class ProcessorStrategyFactoryTests
    {
        public ProcessorStrategyFactoryTests()
        {

        }

        [Fact]
        public void GetStrategy_Returns_InterfaceProcessorStrategy()
        {
            // Arrange
            // Act
            var processorStrategy = ProcessorStrategyFactory.GetStrategy(TypeKind.Interface);

            // Assert
            processorStrategy.Should().NotBeNull();
            processorStrategy.Should().BeOfType<InterfaceProcessorStrategy>();
        }

        [Fact]
        public void GetStrategy_For_AnyType_Returns_InterfaceProcessorStrategy()
        {
            // Arrange
            // Act
            var processorStrategy = ProcessorStrategyFactory.GetStrategy(TypeKind.Dynamic);

            // Assert
            processorStrategy.Should().NotBeNull();
            processorStrategy.Should().BeOfType<InterfaceProcessorStrategy>();
        }

        [Fact]
        public void GetStrategy_Returns_ClassProcessorStrategy()
        {
            // Arrange
            // Act
            var processorStrategy = ProcessorStrategyFactory.GetStrategy(TypeKind.Class);

            // Assert
            processorStrategy.Should().NotBeNull();
            processorStrategy.Should().BeOfType<ClassProcessorStrategy>();
        }
    }
}
