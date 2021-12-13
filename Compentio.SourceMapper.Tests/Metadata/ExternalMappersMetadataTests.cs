using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class ExternalMappersMetadataTests : ExternalMappersMetadataTestBase
    {
        protected override string FakeNamespace => "FakeNamespace";

        protected override string FakeClassName => "FakeClassName";

        protected override string FakeInterfaceName => "FakeInterfaceName";

        protected override string FakeAssemblyName => "FakeAssemblyName";

        [Fact]
        public void Instance_ExternalAssemblies_NotEmpty()
        {
            // Arrange
            _mockAssembly.Setup(a => a.Identity).Returns(GetFakeAssemblyIdentity(FakeAssemblyName));

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

        [Fact]
        public void Instance_ExternalMappers_NotEmpty()
        {
            // Arrange
            _mockNamedType.Setup(n => n.GetAttributes()).Returns(GetFakeClassAttributeData(FakeSourceCode));

            // Act
            var externalMappersMetadata = new ExternalMappersMetadata(new List<IAssemblySymbol> { _mockAssembly.Object });

            // Assert
            externalMappersMetadata.ExternalMappers.Should().NotBeNull();
            externalMappersMetadata.ExternalMappers.Should().NotBeEmpty();
        }
    }
}