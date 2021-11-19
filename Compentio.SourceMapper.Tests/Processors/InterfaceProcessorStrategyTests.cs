using AutoFixture;
using AutoFixture.AutoMoq;
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
        private readonly IFixture _fixture;
        private readonly IProcessorStrategy _processorStrategy;
        private readonly Mock<IMapperMetadata> _sourceMetadataMock;
        private readonly Mock<MappingAttribute> _mockMappingAttribute;

        public InterfaceProcessorStrategyTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());

            _mockMappingAttribute = _fixture.Create<Mock<MappingAttribute>>();
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

        [Fact]
        public void GenerateCode_InverseAttribute_GeneratePartialCode()
        {
            // Arrange
            var methodMetadata = GetValidMethodWithAttributes(_mockMappingAttribute);                       
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata });

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
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.MethodsMetadata).Returns(new List<IMethodMetadata> { methodMetadata });

            // Act
            var result = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Arrange
            result.Diagnostics.Should().NotContain(d => d.DiagnosticDescriptor == SourceMapperDescriptors.PropertyIsNotMapped);
        }

        private IMethodMetadata GetValidMethodWithAttributes(Mock<MappingAttribute> mockMappingAttribute)
        {
            var mockMethodMetadata = _fixture.Create<Mock<IMethodMetadata>>();
            var sourceParameters = mockMethodMetadata.Object.Parameters.First();
            var parameters = GetValidMethodParameters(sourceParameters);
            
            mockMethodMetadata.Setup(m => m.Parameters).Returns(new List<ITypeMetadata> { parameters });
            mockMethodMetadata.Setup(m => m.ReturnType).Returns(parameters);
            mockMethodMetadata.Setup(m => m.MappingAttributes).Returns(new List<MappingAttribute> { mockMappingAttribute.Object });

            mockMappingAttribute.Setup(m => m.CreateInverse).Returns(true);          

            return mockMethodMetadata.Object;
        }

        private ITypeMetadata GetValidMethodParameters(ITypeMetadata sourceTypeMetadata)
        {
            var mockParameters = _fixture.Create<Mock<ITypeMetadata>>();
            mockParameters.Setup(p => p.FullName).Returns(sourceTypeMetadata.FullName);
            mockParameters.Setup(p => p.Location).Returns(sourceTypeMetadata.Location);
            mockParameters.Setup(p => p.Name).Returns(sourceTypeMetadata.Name);
            mockParameters.Setup(p => p.Properties).Returns(new List<IPropertyMetadata> { sourceTypeMetadata.Properties.First() });

            return mockParameters.Object;
        }
    }
}