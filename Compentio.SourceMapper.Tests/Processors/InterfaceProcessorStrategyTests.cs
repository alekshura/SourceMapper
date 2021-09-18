﻿using AutoFixture;
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
        private readonly Mock<IMapperMetadata> _sourceMetadataMock;

        public InterfaceProcessorStrategyTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());

            _sourceMetadataMock = _fixture.Create<Mock<IMapperMetadata>>();
            _sourceMetadataMock.Setup(sourceMetadata => sourceMetadata.TypeKind).Returns(TypeKind.Interface);
            _processorStrategy = ProcessorStrategyFactory.GetStrategy(_sourceMetadataMock.Object);
            
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

            // Act
            var generatedCode = _processorStrategy.GenerateCode(_sourceMetadataMock.Object);

            // Assert
            generatedCode.Should().NotBeNullOrEmpty();
            
        }
    }
}
