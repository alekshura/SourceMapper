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
    public class PropertyMetadataTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPropertyMetadata> _mockSourcePropertyMetadata;
        private readonly Mock<IPropertyMetadata> _mockTargetPropertyMetadata;
        private readonly Mock<IPropertySymbol> _mockPropertySymbol;
        private readonly Mock<Location> _mockLocation;
        private readonly Mock<ISymbol> _mockSymbol;

        public PropertyMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockSourcePropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
            _mockTargetPropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
            _mockPropertySymbol = _fixture.Create<Mock<IPropertySymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
            _mockSymbol = _fixture.Create<Mock<ISymbol>>();
        }

        [Fact]
        public void InstanceForClass_SetFieldsCorrectly()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Name).Returns("Name");
            _mockPropertySymbol.Setup(p => p.Type.ToDisplayString(It.IsAny<SymbolDisplayFormat>())).Returns("FullName");
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Class);
            _mockPropertySymbol.Setup(p => p.Type.SpecialType).Returns(SpecialType.None);
            _mockPropertySymbol.Setup(p => p.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));
            _mockPropertySymbol.Setup(p => p.Type.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));
            _mockSymbol.Setup(s => s.Kind).Returns(SymbolKind.Property);
            _mockSymbol.Setup(s => s.IsStatic).Returns(false);

            // Act
            var propertyMetadata = new PropertyMetadata(_mockPropertySymbol.Object);

            // Assert
            propertyMetadata.Name.Should().Be("Name");
            propertyMetadata.FullName.Should().Be("FullName");
            propertyMetadata.IsClass.Should().BeTrue();
            propertyMetadata.Location.Should().NotBeNull();
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
        public void Swap_CorrectlySwapMetadata()
        {
            // Arrange
            var sourceMetadata = _mockSourcePropertyMetadata.Object;
            var targetMetadata = _mockTargetPropertyMetadata.Object;

            // Act
            PropertyMetadata.Swap(ref sourceMetadata, ref targetMetadata);

            // Assert
            sourceMetadata.Should().BeSameAs(_mockTargetPropertyMetadata.Object);
            targetMetadata.Should().BeSameAs(_mockSourcePropertyMetadata.Object);
        }
    }
}