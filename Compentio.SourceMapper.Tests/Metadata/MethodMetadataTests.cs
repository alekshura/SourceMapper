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
    public class MethodMetadataTests : MethodMetadataTestBase
    {
        private readonly IFixture _fixture;
        private readonly Mock<IMethodSymbol> _mockMethodSymbol;
        private readonly Mock<ISymbol> _mockSymbol;
        private readonly Mock<ITypeSymbol> _mockTypeSymbol;
        private readonly Mock<IParameterSymbol> _mockParameterSymbol;
        private readonly Mock<Location> _mockLocation;

        protected override string FakeClassName => "FakeClassName";

        protected override string FakeMethodName => "FakeMethodName";

        protected override string FakeNamespace => "Compentio.SourceMapper.Tests";

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
        public void Instance_ValidNameField()
        {
            // Arrange
            _mockMethodSymbol.Setup(m => m.ToDisplayParts(It.IsAny<SymbolDisplayFormat>())).Returns(
                ImmutableArray.Create(new SymbolDisplayPart(SymbolDisplayPartKind.MethodName, _mockSymbol.Object, "Name")));

            // Act
            var methodMetadata = new MethodMetadata(_mockMethodSymbol.Object);

            // Assert
            methodMetadata.Name.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Instance_ValidFullNameField()
        {
            // Act
            var methodMetadata = new MethodMetadata(_mockMethodSymbol.Object);

            // Assert
            methodMetadata.FullName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Instance_ValidReturnTypeField()
        {
            // Arrange
            _mockMethodSymbol.Setup(m => m.ReturnType).Returns(_mockTypeSymbol.Object);

            // Act
            var methodMetadata = new MethodMetadata(_mockMethodSymbol.Object);

            // Assert
            methodMetadata.ReturnType.Should().NotBeNull();
        }

        [Fact]
        public void Instance_ValidLocationField()
        {
            // Arrange
            _mockMethodSymbol.Setup(m => m.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var methodMetadata = new MethodMetadata(_mockMethodSymbol.Object);

            // Assert
            methodMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void Instance_NotEmptyParameters()
        {
            // Arrange
            _mockMethodSymbol.Setup(m => m.Parameters).Returns(ImmutableArray.Create(_mockParameterSymbol.Object));

            // Act
            var methodMetadata = new MethodMetadata(_mockMethodSymbol.Object);

            // Assert
            methodMetadata.Parameters.Should().NotBeNull();
            methodMetadata.Parameters.Should().NotBeEmpty();
        }

        [Fact]
        public void Instance_NotEmptyMappingAttributes()
        {
            // Arrange
            _mockMethodSymbol.Setup(m => m.GetAttributes()).Returns(GetFakeAttributeData(FakeSourceCode));

            // Act
            var methodMetadata = new MethodMetadata(_mockMethodSymbol.Object);

            methodMetadata.MappingAttributes.Should().NotBeNull();
            methodMetadata.MappingAttributes.Should().NotBeEmpty();
        }
    }
}