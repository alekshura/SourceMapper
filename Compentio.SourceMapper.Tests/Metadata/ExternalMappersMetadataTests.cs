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
        protected override string MockNamespace => "MockNamespace";

        protected override string MockClassName => "MockClassName";

        protected override string MockInterfaceName => "MockInterfaceName";

        protected override string MockAssemblyName => "MockAssemblyName";

        [Fact]
        public void Instance_ExternalAssemblies_NotEmpty()
        {
            // Arrange
            _mockAssembly.Setup(a => a.Identity).Returns(GetAssemblyIdentityMock(MockAssemblyName));

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
            _mockNamedType.Setup(n => n.GetAttributes()).Returns(GetClassAttributeDataMock(MockSourceCode));

            // Act
            var externalMappersMetadata = new ExternalMappersMetadata(new List<IAssemblySymbol> { _mockAssembly.Object });

            // Assert
            externalMappersMetadata.ExternalMappers.Should().NotBeNull();
            externalMappersMetadata.ExternalMappers.Should().NotBeEmpty();
        }

        [Fact]
        public void Instance_ClassExternalMappers_NotEmpty()
        {
            // Arrange
            _mockNamedType.Setup(n => n.TypeKind).Returns(TypeKind.Class);
            _mockNamedType.Setup(n => n.GetAttributes()).Returns(GetClassAttributeDataMock(MockClassSourceCode));

            // Act
            var externalMappersMetadata = new ExternalMappersMetadata(new List<IAssemblySymbol> { _mockAssembly.Object });

            // Assert
            externalMappersMetadata.ExternalMappers.Should().NotBeNull();
            externalMappersMetadata.ExternalMappers.Should().NotBeEmpty();
            externalMappersMetadata.ExternalMappers.First().TypeKind.Should().Be(TypeKind.Class);
        }

        [Fact]
        public void Instance_InterfaceExternalMappers_NotEmpty()
        {
            // Arrange
            _mockNamedType.Setup(n => n.TypeKind).Returns(TypeKind.Interface);
            _mockNamedType.Setup(n => n.GetAttributes()).Returns(GetInterfaceAttributeDataMock(MockInterfaceSourceCode));

            // Act
            var externalMappersMetadata = new ExternalMappersMetadata(new List<IAssemblySymbol> { _mockAssembly.Object });

            // Assert
            externalMappersMetadata.ExternalMappers.Should().NotBeNull();
            externalMappersMetadata.ExternalMappers.Should().NotBeEmpty();
            externalMappersMetadata.ExternalMappers.First().TypeKind.Should().Be(TypeKind.Interface);
        }
    }
}