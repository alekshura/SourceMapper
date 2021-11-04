using AutoFixture;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Moq;
using Xunit;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class DependencyInjectionStrategyFactoryTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ISourcesMetadata> _mockSourceMetadata;
        private readonly Mock<DependencyInjection> _mockDependencyInjection;

        public DependencyInjectionStrategyFactoryTests()
        {
            _fixture = new Fixture();
            _mockSourceMetadata = _fixture.Create<Mock<ISourcesMetadata>>();
            _mockDependencyInjection = _fixture.Create<Mock<DependencyInjection>>();
        }

        [Fact]
        public void GetStrategy_WithNoneDependencyInjection_ReturnNull()
        {
            // Arrange
            _mockDependencyInjection.Setup(d => d.DependencyInjectionType).Returns(DependencyInjectionType.None);
            _mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(_mockDependencyInjection.Object);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(_mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeNull();
        }

        [Fact]
        public void GetStrategy_ForDotNetCoreDI_ReturnDotnetCoreProcessorStrategy()
        {
            // Arrange
            _mockDependencyInjection.Setup(d => d.DependencyInjectionType).Returns(DependencyInjectionType.DotNetCore);
            _mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(_mockDependencyInjection.Object);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(_mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeOfType<DotnetCoreProcessorStrategy>();
        }

        [Fact]
        public void GetStrategy_ForAutofacDI_ReturnAutofacProcessorStrategy()
        {
            // Arrange
            _mockDependencyInjection.Setup(d => d.DependencyInjectionType).Returns(DependencyInjectionType.Autofac);
            _mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(_mockDependencyInjection.Object);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(_mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeOfType<AutofacProcessorStrategy>();
        }
    }
}