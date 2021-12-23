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
    public class PropertyMetadataTests : PropertyMetadataTestBase
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPropertySymbol> _mockPropertySymbol;
        private readonly Mock<Location> _mockLocation;
        private readonly Mock<ISymbol> _mockSymbol;

        protected override string MockNamespace => "MockNamespace";

        protected override string MockClassName => "MockClassName";

        protected override string MockMethodName => "MockMethodName";

        public PropertyMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockPropertySymbol = _fixture.Create<Mock<IPropertySymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
            _mockSymbol = _fixture.Create<Mock<ISymbol>>();
        }

        [Fact]
        public void InstanceForClass_ValidNameField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Name).Returns("Name");

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.Name.Should().Be("Name");
        }

        [Fact]
        public void InstanceForClass_ValidFullNameField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.ToDisplayString(It.IsAny<SymbolDisplayFormat>())).Returns("FullName");

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.FullName.Should().Be("FullName");
        }

        [Fact]
        public void InstanceForClass_ValidIsClassField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Class);
            _mockPropertySymbol.Setup(p => p.Type.SpecialType).Returns(SpecialType.None);

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.IsClass.Should().BeTrue();
        }

        [Fact]
        public void InstanceForClass_ValidLocationField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void InstanceForClass_NotEmptyProperties()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Class);
            _mockPropertySymbol.Setup(p => p.Type.SpecialType).Returns(SpecialType.None);
            _mockPropertySymbol.Setup(p => p.Type.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));
            _mockSymbol.Setup(s => s.Kind).Returns(SymbolKind.Property);
            _mockSymbol.Setup(s => s.IsStatic).Returns(false);

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.Properties.Should().NotBeEmpty();
        }

        [Fact]
        public void InstanceForInterface_EmptyProperties()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Interface);

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.Properties.Should().BeEmpty();
        }

        [Fact]
        public void InstanceForClass_ValidIgnoreInMapping()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.GetAttributes()).Returns(GetAttributeDataMock(MockSourceCode, MockMethodName));

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.IgnoreInMapping.Should().BeTrue();
        }
    }
}