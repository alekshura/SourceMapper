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
    public class MemberMetadataTests : MemberMetadataTestBase
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPropertySymbol> _mockPropertySymbol;
        private readonly Mock<IFieldSymbol> _mockFieldSymbol;
        private readonly Mock<Location> _mockLocation;
        private readonly Mock<ISymbol> _mockSymbol;

        private const string MockName = "MockName";
        private const string MockFullName = "MockFullName";

        protected override string MockNamespace => "MockNamespace";

        protected override string MockClassName => "MockClassName";

        protected override string MockMethodName => "MockMethodName";

        public MemberMetadataTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true })
                .Customize(new SupportMutableValueTypesCustomization());
            _mockPropertySymbol = _fixture.Create<Mock<IPropertySymbol>>();
            _mockFieldSymbol = _fixture.Create<Mock<IFieldSymbol>>();
            _mockLocation = _fixture.Create<Mock<Location>>();
            _mockSymbol = _fixture.Create<Mock<ISymbol>>();
        }

        [Fact]
        public void InstanceForProperty_ValidNameField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Name).Returns(MockName);

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.Name.Should().Be(MockName);
        }

        [Fact]
        public void InstanceForField_ValidNameField()
        {
            // Arrange
            _mockFieldSymbol.Setup(p => p.Name).Returns(MockName);

            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.Name.Should().Be(MockName);
        }

        [Fact]
        public void InstanceForProperty_ValidFullNameField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.ToDisplayString(It.IsAny<SymbolDisplayFormat>())).Returns(MockFullName);

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.FullName.Should().Be(MockFullName);
        }

        [Fact]
        public void InstanceForField_ValidFullNameField()
        {
            // Arrange
            _mockFieldSymbol.Setup(p => p.Type.ToDisplayString(It.IsAny<SymbolDisplayFormat>())).Returns(MockFullName);

            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.FullName.Should().Be(MockFullName);
        }

        [Fact]
        public void InstanceForProperty_ValidIsClassField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Class);
            _mockPropertySymbol.Setup(p => p.Type.SpecialType).Returns(SpecialType.None);

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.IsClass.Should().BeTrue();
        }

        [Fact]
        public void InstanceForField_ValidIsClassField()
        {
            // Arrange
            _mockFieldSymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Class);
            _mockFieldSymbol.Setup(p => p.Type.SpecialType).Returns(SpecialType.None);

            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.IsClass.Should().BeTrue();
        }

        [Fact]
        public void InstanceForProperty_ValidLocationField()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void InstanceForField_ValidLocationField()
        {
            // Arrange
            _mockFieldSymbol.Setup(p => p.Locations).Returns(ImmutableArray.Create(_mockLocation.Object));

            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.Location.Should().NotBeNull();
        }

        [Fact]
        public void InstanceForProperty_NotEmptyProperties()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Class);
            _mockPropertySymbol.Setup(p => p.Type.SpecialType).Returns(SpecialType.None);
            _mockPropertySymbol.Setup(p => p.Type.GetMembers()).Returns(ImmutableArray.Create(_mockSymbol.Object));
            _mockSymbol.Setup(s => s.Kind).Returns(SymbolKind.Property);
            _mockSymbol.Setup(s => s.IsStatic).Returns(false);

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.Properties.Should().NotBeEmpty();
        }

        [Fact]
        public void InstanceForProperty_EmptyProperties()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.Type.TypeKind).Returns(TypeKind.Interface);

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.Properties.Should().BeEmpty();
        }

        [Fact]
        public void InstanceForField_EmptyProperties()
        {
            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.Properties.Should().BeEmpty();
        }

        [Fact]
        public void InstanceForProperty_ValidIgnoreInMapping()
        {
            // Arrange
            _mockPropertySymbol.Setup(p => p.GetAttributes()).Returns(GetAttributeDataMock(MockSourceCode, MockMethodName));

            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.IgnoreInMapping.Should().BeTrue();
        }

        [Fact]
        public void InstanceForField_ValidIgnoreInMapping()
        {
            // Arrange
            _mockFieldSymbol.Setup(p => p.GetAttributes()).Returns(GetAttributeDataMock(MockSourceCode, MockMethodName));

            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.IgnoreInMapping.Should().BeTrue();
        }

        [Fact]
        public void InstanceForField_ValidIsStatic()
        {
            // Arrange
            _mockFieldSymbol.Setup(p => p.IsStatic).Returns(true);

            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.IsStatic.Should().BeTrue();
        }

        [Fact]
        public void InstanceForProperty_IsPropertyMember()
        {
            // Act
            var memberMetadata = new MemberMetadata(_mockPropertySymbol.Object);

            // Assert
            memberMetadata.MemberType.Should().Be(MemberType.Property);
        }

        [Fact]
        public void InstanceForField_IsFieldMember()
        {
            // Act
            var memberMetadata = new MemberMetadata(_mockFieldSymbol.Object);

            // Assert
            memberMetadata.MemberType.Should().Be(MemberType.Field);
        }

        [Fact]
        public void Instance_MemberUnknown()
        {
            // Act
            var memberMetadata = new MemberMetadata(_mockSymbol.Object);

            // Assert
            memberMetadata.MemberType.Should().Be(MemberType.Unknown);
        }
    }
}