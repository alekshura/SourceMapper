using AutoFixture;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class AutofacProcessorStrategyTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ISourcesMetadata> _mockSourceMetadata;
        private const string BuilderRegisterService = "builder.RegisterType";

        public AutofacProcessorStrategyTests()
        {
            _fixture = new Fixture();
            _mockSourceMetadata = _fixture.Create<Mock<ISourcesMetadata>>();
            _mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(new SourceMapper.Processors.DependencyInjection.DependencyInjection() { DependencyInjectionType = DependencyInjectionType.Autofac });
        }

        [Fact]
        public void GenerateCode_WithoutMappers_ReturnProperResult()
        {
            // Arrange
            _mockSourceMetadata.Setup(m => m.Mappers).Returns((IReadOnlyCollection<IMapperMetadata>)null);

            // Act
            var autofacProcessorStrategy = new AutofacProcessorStrategy();
            var result = autofacProcessorStrategy.GenerateCode(_mockSourceMetadata.Object);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void GenerateCode_WithoutMappers_NotRegisterAnyService()
        {
            // Arrange
            _mockSourceMetadata.Setup(m => m.Mappers).Returns((IReadOnlyCollection<IMapperMetadata>)null);

            // Act
            var autofacProcessorStrategy = new AutofacProcessorStrategy();
            var result = autofacProcessorStrategy.GenerateCode(_mockSourceMetadata.Object);

            // Assert
            result.GeneratedCode.Should().NotContain(BuilderRegisterService);
        }

        [Fact]
        public void GenerateCode_WithMapper_RegisterService()
        {
            // Arrange
            var mockMapperMetadata = _fixture.Create<Mock<IMapperMetadata>>();

            mockMapperMetadata.Setup(m => m.Name).Returns("Name");
            mockMapperMetadata.Setup(m => m.Namespace).Returns("Namespace");
            mockMapperMetadata.Setup(m => m.TargetClassName).Returns("TargetClassName");

            _mockSourceMetadata.Setup(s => s.Mappers).Returns(new ReadOnlyCollection<IMapperMetadata>(new List<IMapperMetadata> { mockMapperMetadata.Object }));

            // Act
            var autofacProcessorStrategy = new AutofacProcessorStrategy();
            var result = autofacProcessorStrategy.GenerateCode(_mockSourceMetadata.Object);

            // Assert
            result.GeneratedCode.Should().Contain(BuilderRegisterService);
        }
    }
}