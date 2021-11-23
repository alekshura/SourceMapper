using AutoFixture;
using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Diagnostics;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Processors
{
    public class InterfaceProcessorStrategyTests : ProcessorStrategyTestBase
    {
        private readonly IProcessorStrategy _processorStrategy;
        private readonly Mock<IMapperMetadata> _sourceMetadataMock;

        public InterfaceProcessorStrategyTests()
        {
            _sourceMetadataMock = _fixture.Create<Mock<IMapperMetadata>>();
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TypeKind).Returns(TypeKind.Interface);
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.Name).Returns("MapperName");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.Namespace).Returns("Namespace");
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TargetClassName).Returns("TargetClassName");
            _processorStrategy = ProcessorStrategyFactory.GetStrategy(_sourceMetadataMock.Object);
        }

        [Fact]
        public void GenerateCode_Returns_NotEmpty_Code()
        {
            // Arrange
            var methodsMetadata = _fixture.CreateMany<IMethodMetadata>();
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(methodsMetadata);

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Assert
            result.GeneratedCode.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateCode_Check_Diagnostics()
        {
            // Arrange
            var methodsMetadata = _fixture.CreateMany<IMethodMetadata>();
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(methodsMetadata);

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);
            var generatorResult = GetGeneratedOutput(result.GeneratedCode);

            // Assert
            result.GeneratedCode.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateCode_InverseAttribute_GeneratePartialCode()
        {
            // Arrange
            var methodMetadata = GetValidMethodWithAttributes(_mockMappingAttribute);
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata.Object });

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Arrange
            result.GeneratedCode.Should().NotBeNullOrEmpty();
            result.GeneratedCode.Should().Contain($"public partial interface {_sourceMetadataMock.Object.Name}");
        }

        [Fact]
        public void GenerateCode_InverseAttribute_ValidPropertyMapping()
        {
            // Arrange
            var methodMetadata = GetValidMethodWithAttributes(_mockMappingAttribute);
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata.Object });

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Arrange
            result.Diagnostics.Should().NotContain(d => d.DiagnosticDescriptor == SourceMapperDescriptors.PropertyIsNotMapped);
        }

        [Fact]
        public void GenerateCode_ReportEmptyInverseMethodName()
        {
            // Arrange
            var methodMetadata = GetValidMethodWithAttributes(_mockMappingAttribute);
            _mockMappingAttribute.Setup(m => m.InverseMethodName).Returns(string.Empty);
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata.Object });

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Arrange
            result.Diagnostics.Should().Contain(d => d.DiagnosticDescriptor == SourceMapperDescriptors.ExpectedInverseMethodName);
        }

        [Fact]
        public void GenerateCode_UnmatchedData_ReportNotMapped()
        {
            // Arrange
            var methodMetadata = GetValidMethodWithAttributes(_mockMappingAttribute);
            methodMetadata.Setup(m => m.ReturnType.Properties).Returns(_fixture.Create<IEnumerable<IPropertyMetadata>>());
            methodMetadata.Setup(m => m.Parameters).Returns(_fixture.Create<IEnumerable<ITypeMetadata>>());
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata.Object });

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Arrange
            result.Diagnostics.Should().Contain(d => d.DiagnosticDescriptor == SourceMapperDescriptors.PropertyIsNotMapped);
        }

        [Fact]
        public void GenerateCode_NullMappingAttributes_ReportUnexpectedException()
        {
            // Arrange
            var methodMetadata = GetValidMethodWithAttributes(_mockMappingAttribute);
            methodMetadata.Setup(m => m.MappingAttributes).Returns((IEnumerable<MappingAttribute>)null);
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata.Object });

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Arrange
            result.Diagnostics.Should().Contain(d => d.DiagnosticDescriptor == SourceMapperDescriptors.UnexpectedError);
        }

        private Mock<IMethodMetadata> GetValidMethodWithAttributes(Mock<MappingAttribute> mockMappingAttribute)
        {
            var mockMethodMetadata = _fixture.Create<Mock<IMethodMetadata>>();
            var sourceParameters = mockMethodMetadata.Object.Parameters.First();
            var mockParameters = GetValidMethodParameters(sourceParameters);

            mockMethodMetadata.Setup(m => m.Parameters).Returns(new List<ITypeMetadata> { mockParameters.Object });
            mockMethodMetadata.Setup(m => m.ReturnType).Returns(mockParameters.Object);
            mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute> { mockMappingAttribute.Object });

            mockMappingAttribute.Setup(m => m.CreateInverse).Returns(true);

            return mockMethodMetadata;
        }

        private Mock<ITypeMetadata> GetValidMethodParameters(ITypeMetadata sourceTypeMetadata)
        {
            var mockParameters = _fixture.Create<Mock<ITypeMetadata>>();
            mockParameters.Setup(p => p.Name).Returns(sourceTypeMetadata.Name);
            mockParameters.Setup(p => p.FullName).Returns(sourceTypeMetadata.FullName);
            mockParameters.Setup(p => p.Location).Returns(sourceTypeMetadata.Location);
            mockParameters.Setup(p => p.Properties).Returns(new List<IPropertyMetadata> { sourceTypeMetadata.Properties.First() });

            return mockParameters;
        }
    }
}