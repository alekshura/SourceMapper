using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;
using FluentAssertions;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class AutofacProcessorStrategyTests
    {
        private const string BUILDER_REGISTER_SERVICE = "builder.RegisterType";

        [Fact]
        public void GenerateCode_WithoutMappers_ReturnProperResult()
        {
            // Arrange
            var mockSourceMetadata = GetMockSourceMetadata();
            mockSourceMetadata.Setup(m => m.Mappers).Returns((IReadOnlyCollection<IMapperMetadata>)null);

            // Act
            var autofacProcessorStrategy = new AutofacProcessorStrategy();
            var result = autofacProcessorStrategy.GenerateCode(mockSourceMetadata.Object);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void GenerateCode_WithoutMappers_NotRegisterAnyService()
        {
            // Arrange
            var mockSourceMetadata = GetMockSourceMetadata();
            mockSourceMetadata.Setup(m => m.Mappers).Returns((IReadOnlyCollection<IMapperMetadata>)null);

            // Act
            var autofacProcessorStrategy = new AutofacProcessorStrategy();
            var result = autofacProcessorStrategy.GenerateCode(mockSourceMetadata.Object);

            // Assert
            result.GeneratedCode.Should().NotContain(BUILDER_REGISTER_SERVICE);
        }

        [Fact]
        public void GenerateCode_WithMapper_RegisterService()
        {
            // Arrange
            var mockSourceMetadata = GetMockSourceMetadata();
            var mockMapperMetadata = new Mock<IMapperMetadata>();

            mockMapperMetadata.Setup(m => m.Name).Returns("Name");
            mockMapperMetadata.Setup(m => m.Namespace).Returns("Namespace");
            mockMapperMetadata.Setup(m => m.TargetClassName).Returns("TargetClassName");

            mockSourceMetadata.Setup(s => s.Mappers).Returns(new ReadOnlyCollection<IMapperMetadata>(new List<IMapperMetadata> { mockMapperMetadata.Object }));

            // Act
            var autofacProcessorStrategy = new AutofacProcessorStrategy();
            var result = autofacProcessorStrategy.GenerateCode(mockSourceMetadata.Object);

            // Assert
            result.GeneratedCode.Should().Contain(BUILDER_REGISTER_SERVICE);
        }

        private static Mock<ISourcesMetadata> GetMockSourceMetadata()
        {
            var mockSourceMetadata = new Mock<ISourcesMetadata>();
            mockSourceMetadata.Setup(m => m.DependencyInjection).Returns(new DependencyInjection() { DependencyInjectionType = DependencyInjectionType.Autofac });

            return mockSourceMetadata;
        }
    }
}