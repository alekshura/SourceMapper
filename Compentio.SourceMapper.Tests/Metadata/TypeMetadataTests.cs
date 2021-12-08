using AutoFixture;
using AutoFixture.AutoMoq;
using Compentio.SourceMapper.Metadata;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class TypeMetadataTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITypeSymbol> _mockTypeSymbol;
        private readonly Mock<Location> _mockLocation;
        private readonly Mock<ISymbol> _mockSymbol;
        private readonly Mock<IPropertyMetadata> _mockPropertyMetadata;

        public TypeMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockTypeSymbol = _fixture.Create<Mock<ITypeSymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
            _mockSymbol = _fixture.Create<Mock<ISymbol>>();
            _mockPropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
        }

        [Fact]
        public void Instance_ValidNameField()
        {
            // Arrange
            var fakeName = "FakeName";
            _mockTypeSymbol.Setup(p => p.Name).Returns(fakeName);

            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);

            // Assert
            typeMetadata.Name.Should().Be(fakeName);
        }

        [Fact]
        public void Instance_ValidFullNameField()
        {
            // Arrange
            var fakeFullName = "FakeFullName";
            _mockTypeSymbol.Setup(t => t.ToDisplayString(It.IsAny<SymbolDisplayFormat>())).Returns(fakeFullName);

            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);

            // Assert
            typeMetadata.FullName.Should().Be(fakeFullName);
        }

        [Fact]
        public void Instance_ValidType()
        {
            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);

            // Assert
            typeMetadata.Type.Should().BeSameAs(_mockTypeSymbol.Object);
        }

        [Fact]
        public void Instance_ValidLocationField()
        {
            // Arrange
            _mockTypeSymbol.Setup(p => p.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);

            // Assert
            typeMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void Instanc_NotEmptyProperties()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));
            _mockSymbol.Setup(s => s.Kind).Returns(SymbolKind.Property);
            _mockSymbol.Setup(s => s.IsStatic).Returns(false);

            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);

            // Assert
            typeMetadata.Properties.Should().NotBeEmpty();
        }

        [Fact]
        public void Instanc_EmptyProperties()
        {
            // Arrange
            _mockTypeSymbol.Setup(t => t.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));
            _mockSymbol.Setup(s => s.Kind).Returns(SymbolKind.ErrorType);
            _mockSymbol.Setup(s => s.IsStatic).Returns(true);

            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);

            // Assert
            typeMetadata.Properties.Should().BeEmpty();
        }

        [Fact]
        public void FlattenProperties_ValidFlatten()
        {
            // Arrange
            var limitedPropertyMetadata = _fixture.Create<Mock<IPropertyMetadata>>();
            limitedPropertyMetadata.Setup(l => l.Properties).Returns(new List<IPropertyMetadata>());

            _mockPropertyMetadata.Setup(p => p.Properties).Returns(
                new List<IPropertyMetadata> 
                { 
                    limitedPropertyMetadata.Object, 
                    limitedPropertyMetadata.Object, 
                    limitedPropertyMetadata.Object 
                });

            var objectsCount = 4; //  Primary object plus all objects from properties list

            // Act
            var typeMetadata = new TypeMetadata(_mockTypeSymbol.Object);
            var result = typeMetadata.FlattenProperties(new List<IPropertyMetadata> { _mockPropertyMetadata.Object });

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(objectsCount);
        }
    }
}