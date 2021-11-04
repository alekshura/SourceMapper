using Xunit;
using Moq;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class DependencyInjectionStrategyFactoryTests
    {
        [Fact]
        public void GetStrategy_WithNoneDependencyInjection_ReturnNull()
        {
            // Arrange
            var mockSourceMetadata = GetMockSourcesMetadata(DependencyInjectionType.None);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeNull();
        }

        [Fact]
        public void GetStrategy_ForDotNetCoreDI_ReturnDotnetCoreProcessorStrategy()
        {

            // Arrange
            var mockSourceMetadata = GetMockSourcesMetadata(DependencyInjectionType.DotNetCore);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeOfType<DotnetCoreProcessorStrategy>();
        }

        [Fact]
        public void GetStrategy_ForAutofacDI_ReturnAutofacProcessorStrategy()
        {

            // Arrange          
            var mockSourceMetadata = GetMockSourcesMetadata(DependencyInjectionType.Autofac);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeOfType<AutofacProcessorStrategy>();
        }

        private static Mock<ISourcesMetadata> GetMockSourcesMetadata(DependencyInjectionType dependencyInjectionType)
        {
            var mockDependencyInjection = GetMockDependencyInjection(dependencyInjectionType);
            var mockSourceMetadata = new Mock<ISourcesMetadata>();
            mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(mockDependencyInjection.Object);

            return mockSourceMetadata;
        }

        private static Mock<DependencyInjection> GetMockDependencyInjection(DependencyInjectionType dependencyInjectionType)
        {
            var mockDependencyInjection = new Mock<DependencyInjection>();
            mockDependencyInjection.Setup(d => d.DependencyInjectionType).Returns(dependencyInjectionType);

            return mockDependencyInjection;
        }
    }
}
