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
    public class SourcesMetadataTests
    {
        private const string AutofacAssemblyName = "Autofac.Extensions.DependencyInjection";
        private const string DotNetCoreAssemblyName = "Microsoft.Extensions.DependencyInjection";
        private const string StructureMapAssemblyName = "StructureMap.Microsoft.DependencyInjection";
        private const string MockAssemblyName = "FakeAssemblyName";
        private readonly IFixture _fixture;
        private readonly Mock<IMapperMetadata> _mockMapperMetadata;
        private readonly Mock<IEnumerable<IMapperMetadata>> _mockMappersMetadata;

        public SourcesMetadataTests()
        {
            _fixture = new Fixture();
            _mockMapperMetadata = _fixture.Create<Mock<IMapperMetadata>>();
            _mockMappersMetadata = _fixture.Create<Mock<IEnumerable<IMapperMetadata>>>();
        }

        [Fact]
        public void DependencyInjection_DotNetCoreDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(DotNetCoreAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.DotNetCore);
        }

        [Fact]
        public void DependencyInjection_AutofacDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(AutofacAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.Autofac);
        }

        [Fact]
        public void DependencyInjection_StructureMapDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(StructureMapAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.StructureMap);
        }

        [Fact]
        public void DependencyInjection_NoDependencyInjection()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

            // Act
            var result = SourcesMetadata.Create(assemblies);

            // Assert
            result.DependencyInjection.Should().NotBeNull();
            result.DependencyInjection.DependencyInjectionType.Should().Be(DependencyInjectionType.None);
        }

        [Fact]
        public void AddOrUpdate_AddMapper()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemblies);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void AddOrUpdate_DuplicateMappers()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemblies);
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
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };
            var mockMapperMetadata = _fixture.Build<Mock<IMapperMetadata>>()
                .WithAutoProperties()
                .Without(n => n.DefaultValue)
                .Create();
            mockMapperMetadata.Setup(m => m.Name).Returns(_mockMapperMetadata.Object.Name);

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemblies);
            sourcesMetadata.AddOrUpdate(_mockMapperMetadata.Object);
            sourcesMetadata.AddOrUpdate(mockMapperMetadata.Object);

            // Assert
            sourcesMetadata.Mappers.Should().NotBeNullOrEmpty();
            sourcesMetadata.Mappers.Should().ContainSingle();
        }

        [Fact]
        public void AddRange_AddMappers()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemblies);
            sourcesMetadata.AddRange(_mockMappersMetadata.Object);

            // Assert
            sourcesMetadata.Mappers.Should().BeEquivalentTo(_mockMappersMetadata.Object);
        }

        [Fact]
        public void AddRange_EmptyMappers()
        {
            // Arrange
            var assemblies = new List<AssemblyIdentity> { GetAssemblyIdentityMock(MockAssemblyName) };

            // Act
            var sourcesMetadata = SourcesMetadata.Create(assemblies);
            sourcesMetadata.AddRange(null);

            // Assert
            sourcesMetadata.Mappers.Should().BeNullOrEmpty();
        }

        private static AssemblyIdentity GetAssemblyIdentityMock(string name)
        {
            return new AssemblyIdentity(name: name);
        }
    }
}