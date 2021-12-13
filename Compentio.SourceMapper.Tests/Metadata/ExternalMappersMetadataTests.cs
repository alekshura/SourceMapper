using AutoFixture;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class ExternalMappersMetadataTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAssemblySymbol> _mockAssembly;
        private const string FakeAssemblyName = "FakeAssemblyName";

        public ExternalMappersMetadataTests()
        {
            _fixture = new Fixture();
            _mockAssembly = _fixture.Create<Mock<IAssemblySymbol>>();
        }

        [Fact]
        public void Instance_ExternalAssemblies_NotEmpty()
        {
            // Arrange
            _mockAssembly.Setup(a => a.Identity).Returns(GetFakeAssemblyIdentity());

            // Act
            var externalMappersMetadata = new ExternalMappersMetadata(new List<IAssemblySymbol> { _mockAssembly.Object });

            // Assert
            externalMappersMetadata.ExternalAssemblies.Should().NotBeNull();
            externalMappersMetadata.ExternalAssemblies.Should().NotBeEmpty();
        }

        [Fact]
        public void Instance_ExternalAssemblies_NoAssemblies()
        {
            // Act
            var externalMappersMetadata = new ExternalMappersMetadata(null);

            // Assert
            externalMappersMetadata.ExternalAssemblies.Should().BeNull();
        }

        private static AssemblyIdentity GetFakeAssemblyIdentity()
        {
            return new AssemblyIdentity(FakeAssemblyName);
        }
    }
}