using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Immutable;
using Xunit;

namespace Compentio.SourceMapper.Tests.Metadata
{
    public class MethodMetadataTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMethodSymbol> _mockMethodSymbol;
        private readonly Mock<ISymbol> _mockSymbol;
        private readonly Mock<ITypeSymbol> _mockTypeSymbol;
        private readonly Mock<IParameterSymbol> _mockParameterSymbol;
        private readonly Mock<Location> _mockLocation;

        public MethodMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockMethodSymbol = _fixture.Create<Mock<IMethodSymbol>>();
            _mockSymbol = _fixture.Create<Mock<ISymbol>>();
            _mockTypeSymbol = _fixture.Create<Mock<ITypeSymbol>>();
            _mockParameterSymbol = _fixture.Create<Mock<IParameterSymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
        }

        [Fact]
        public void Instance_SetFieldsCorrectly()
        {
            // Arrange
            _mockMethodSymbol.Setup(m => m.ReturnType).Returns(_mockTypeSymbol.Object);
            _mockMethodSymbol.Setup(m => m.ToDisplayParts(It.IsAny<SymbolDisplayFormat>())).Returns(
                ImmutableArray.Create(new SymbolDisplayPart(SymbolDisplayPartKind.MethodName, _mockSymbol.Object, "Name")));
            _mockMethodSymbol.Setup(m => m.Parameters).Returns(ImmutableArray.Create(_mockParameterSymbol.Object));
            _mockMethodSymbol.Setup(m => m.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var methodMetadata = new FakeMethodMetadata(_mockMethodSymbol.Object);

            // Assert
            methodMetadata.Name.Should().NotBeNullOrEmpty();
            methodMetadata.FullName.Should().NotBeNullOrEmpty();
            methodMetadata.ReturnType.Should().NotBeNull();
            methodMetadata.Parameters.Should().NotBeNull();
            methodMetadata.Parameters.Should().NotBeEmpty();
            methodMetadata.Location.Should().NotBeNull();
            methodMetadata.MappingAttributes.Should().NotBeNull();
            methodMetadata.MappingAttributes.Should().NotBeEmpty();
        }
    }
}