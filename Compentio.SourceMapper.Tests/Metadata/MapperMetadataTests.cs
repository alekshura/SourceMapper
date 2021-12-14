using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Immutable;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class MapperMetadataTests : MapperMetadataTestBase
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITypeSymbol> _mockTypeSymbol;
        private readonly Mock<INamespaceSymbol> _mockNamespaceSymbol;
        private readonly Mock<Location> _mockLocation;

        protected override string FakeClassName => "FakeClassName";

        protected override string FakeNamespace => "Compentio.SourceMapper.Tests";

        protected override string FakeInterfaceName => "FakeInterfaceName";

        public MapperMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockTypeSymbol = _fixture.Create<Mock<ITypeSymbol>>();
            _mockNamespaceSymbol = _fixture.Create<Mock<INamespaceSymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
        }

        [Fact]
        public void Instance_ValidTypeKind()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.TypeKind).Returns(TypeKind.Class);

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TypeKind.Should().Be(_mockTypeSymbol.Object.TypeKind);
        }

        [Fact]
        public void Instance_Class_ValidTargetClassName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetFakeClassAttributeData(FakeClassSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TargetClassName.Should().Be(FakeClassName);
        }

        [Fact]
        public void Instance_Interface_ValidTargetClassName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.Name).Returns(FakeInterfaceName);
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetFakeInterfaceAttributeData(FakeInterfaceSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TargetClassName.Should().Be(FakeInterfaceName);
        }

        [Fact]
        public void Instance_EmptyMapperName_ValidTargetClassName()
        {
            // Arrange

            _mockTypeSymbol.Setup(t => t.Name).Returns(FakeClassName);
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetFakeClassAttributeData(FakeSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TargetClassName.Should().Be($"{FakeClassName}Impl");
        }

        [Fact]
        public void Instance_ValidName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.Name).Returns(FakeClassName);

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.Name.Should().Be(FakeClassName);
        }

        [Fact]
        public void Instance_ValidFileName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetFakeClassAttributeData(FakeClassSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.FileName.Should().Be($"{FakeClassName}.cs");
        }

        [Fact]
        public void Instance_ValidNamespace()
        {
            // Arrange
            var namespaceSymbol = _mockNamespaceSymbol.Object;
            _mockTypeSymbol.Setup(t => t.ContainingNamespace).Returns(namespaceSymbol);

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.Namespace.Should().Be(namespaceSymbol.ToString());
        }

        [Fact]
        public void Instance_ValidLocation()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void Instance_Class_NotEmptyMethodsMetadata()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.GetMembers()).Returns(GetFakeClassMethods(FakeClassSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.MethodsMetadata.Should().NotBeNull();
            mapperMetadata.MethodsMetadata.Should().NotBeEmpty();
        }

        [Fact]
        public void Instance_IsExternal_ReturnTrue()
        {
            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object, true);

            // Assert
            mapperMetadata.IsExternal.Should().BeTrue();
        }
    }
}