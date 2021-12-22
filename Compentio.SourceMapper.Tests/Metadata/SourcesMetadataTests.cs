using AutoFixture;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Linq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class SourcesMetadataTests : SourcesMetadataTestBase
    {
        private readonly Mock<IMapperMetadata> _mockMapperMetadata;

        protected override string MockNamespace => "MockNamespace";

        protected override string MockClassName => "MockClassName";

        protected override string MockInterfaceName => "MockInterfaceName";

        protected override string MockAssemblyName => "MockAssemblyName";

        public SourcesMetadataTests()
        {
            _mockMapperMetadata = _fixture.Create<Mock<IMapperMetadata>>();
        }

        [Fact]
        public void DependencyInjection_DotNetCoreDependencyInjection()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(DotNetCoreAssemblyName);

            // Act
            var result = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.DotNetCore);
        }

        [Fact]
        public void DependencyInjection_AutofacDependencyInjection()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(AutofacAssemblyName);

            // Act
            var result = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.Autofac);
        }

        [Fact]
        public void DependencyInjection_StructureMapDependencyInjection()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(StructureMapAssemblyName);

            // Act
            var result = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.StructureMap);
        }

        [Fact]
        public void DependencyInjection_NoDependencyInjection()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);

            // Act
            var result = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.None);
        }

        [Fact]
        public void AddOrUpdate_AddMapper()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void AddOrUpdate_DuplicateMappers()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNullOrEmpty();
            sourcesMetadata.Mappers.Should().ContainSingle();
        }

        [Fact]
        public void AddOrUpdate_UpdateMappers()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);
            var mockMapperMetadata = _fixture.Build<Mock<IMapperMetadata>>()
                .WithAutoProperties()
                .Without(n => n.DefaultValue)
                .Create();
            mockMapperMetadata.Setup(m => m.Name).Returns(_mockMapperMetadata.Object.Name);

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);
            sourcesMetadata.AddOrUpdate(mockMapperMetadata.Object);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNullOrEmpty();
            sourcesMetadata.Mappers.Should().ContainSingle();
        }

        [Fact]
        public void Instance_ReferencedMappers_NotEmpty()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNull();
            sourcesMetadata.Mappers.Should().NotBeEmpty();
        }

        [Fact]
        public void Instance_ClassReferencedMappers_NotEmpty()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);

            _mockNamedType.Setup(n => n.TypeKind).Returns(TypeKind.Class);
            _mockNamedType.Setup(n => n.GetAttributes()).Returns(GetClassAttributeDataMock(MockClassSourceCode));

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNull();
            sourcesMetadata.Mappers.Should().NotBeEmpty();
            sourcesMetadata.Mappers.First().TypeKind.Should().Be(TypeKind.Class);
        }

        [Fact]
        public void Instance_InterfaceReferencedMappers_NotEmpty()
        {
            // Arrange
            var assemlyIdentities = GetAssemblyIdentityCollectionMock(MockAssemblyName);

            _mockNamedType.Setup(n => n.TypeKind).Returns(TypeKind.Interface);
            _mockNamedType.Setup(n => n.GetAttributes()).Returns(GetInterfaceAttributeDataMock(MockInterfaceSourceCode));

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemlyIdentities, _assemblySymbols);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNull();
            sourcesMetadata.Mappers.Should().NotBeEmpty();
            sourcesMetadata.Mappers.First().TypeKind.Should().Be(TypeKind.Interface);
        }
    }
}