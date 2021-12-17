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

        protected override string MockClassName => "MockClassName";

        protected override string MockNamespace => "Compentio.SourceMapper.Tests";

        protected override string MockInterfaceName => "MockInterfaceName";

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
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetClassAttributeDataMock(MockClassSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TargetClassName.Should().Be(MockClassName);
        }

        [Fact]
        public void Instance_Interface_ValidTargetClassName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.Name).Returns(MockInterfaceName);
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetInterfaceAttributeDataMock(MockInterfaceSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TargetClassName.Should().Be(MockInterfaceName);
        }

        [Fact]
        public void Instance_EmptyMapperName_ValidTargetClassName()
        {
            // Arrange

            _mockTypeSymbol.Setup(t => t.Name).Returns(MockClassName);
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetClassAttributeDataMock(MockSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.TargetClassName.Should().Be($"{MockClassName}Impl");
        }

        [Fact]
        public void Instance_ValidName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.Name).Returns(MockClassName);

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.Name.Should().Be(MockClassName);
        }

        [Fact]
        public void Instance_ValidFileName()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.GetAttributes()).Returns(GetClassAttributeDataMock(MockClassSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.FileName.Should().Be($"{MockClassName}.cs");
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
            _mockTypeSymbol.Setup(t => t.GetMembers()).Returns(GetClassMethodsMock(MockClassSourceCode));

            // Act
            var mapperMetadata = new MapperMetadata(_mockTypeSymbol.Object);

            // Assert
            mapperMetadata.MethodsMetadata.Should().NotBeNull();
            mapperMetadata.MethodsMetadata.Should().NotBeEmpty();
        }
    }
}