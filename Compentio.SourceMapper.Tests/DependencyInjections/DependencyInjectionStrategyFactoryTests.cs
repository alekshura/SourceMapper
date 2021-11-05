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
        public void GetStrategy_ForDotNetCoreType_ReturnDotnetCoreProcessorStrategy()
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
        public void GetStrategy_ForAutofacType_ReturnAutofacProcessorStrategy()
        {
            // Arrange
            _mockDependencyInjection.Setup(d => d.DependencyInjectionType).Returns(DependencyInjectionType.Autofac);
            _mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(_mockDependencyInjection.Object);

            //Act
            var dependencyInjectionStrategy = DependencyInjectionStrategyFactory.GetStrategy(_mockSourceMetadata.Object);

            //Assert
            dependencyInjectionStrategy.Should().BeOfType<AutofacProcessorStrategy>();
        }

        [Fact]
        public void FactoryStrategies_ContainStrategy_DotNetCore()
        {
            // Act
            var result = DependencyInjectionStrategyFactory.DependencyInjectionStrategies.ContainsKey(DependencyInjectionType.DotNetCore);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void FactoryStrategies_ContainStrategy_Autofac()
        {
            // Act
            var result = DependencyInjectionStrategyFactory.DependencyInjectionStrategies.ContainsKey(DependencyInjectionType.Autofac);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void FactoryStrategies_NotContainStrategy_None()
        {
            // Act
            var result = DependencyInjectionStrategyFactory.DependencyInjectionStrategies.ContainsKey(DependencyInjectionType.None);

            //Assert
            result.Should().BeFalse();
        }
    }
}