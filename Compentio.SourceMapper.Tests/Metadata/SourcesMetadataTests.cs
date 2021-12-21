using AutoFixture;
using Compentio.SourceMapper.Metadata;
using Compentio.SourceMapper.Processors.DependencyInjection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class SourcesMetadataTests : SourcesMetadataTestBase
    {
        private const string AutofacAssemblyName = "Autofac.Extensions.DependencyInjection";
        private const string DotNetCoreAssemblyName = "Microsoft.Extensions.DependencyInjection";
        private const string StructureMapAssemblyName = "StructureMap.Microsoft.DependencyInjection";
        private readonly Mock<IMapperMetadata> _mockMapperMetadata;

        protected override string MockAssemblyName => "MockAssemblyName";

        public SourcesMetadataTests()
        {
            _mockMapperMetadata = _fixture.Create<Mock<IMapperMetadata>>();
        }

        [Fact]
        public void DependencyInjection_DotNetCoreDependencyInjection()
        {
            // Arrange
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(DotNetCoreAssemblyName) };

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
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(AutofacAssemblyName) };

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
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(StructureMapAssemblyName) };

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
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

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
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

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
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

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
            var assemlyIdentities = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };
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
    }
}